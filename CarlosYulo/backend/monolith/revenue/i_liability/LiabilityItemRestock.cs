using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.backend.monolith.shop;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.revenue.i_liability;

public class LiabilityItemRestock
{
    private DatabaseConnection dbConnection;

    public LiabilityItemRestock(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public ItemRestock? GenerateItemRestock(Item item, int? quantity, out string message)
    {
        if (item is null)
        {
            message = "Item is null";
            return null;
        }

        if (quantity is null || quantity <= 0)
        {
            message = "Quantity is zero or null";
            return null;
        }

        ItemRestock restock = null;
        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcLiabilityItemRestockCreate",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_item_id", item.ItemId);
                command.Parameters.AddWithValue("p_item_name", item.ItemName);
                command.Parameters.AddWithValue("p_total_cost", (double)quantity * item.ItemLiabilityCost);
                command.Parameters.AddWithValue("p_restock_amount", quantity);
                command.Parameters.AddWithValue("p_liability_cost", item.ItemLiabilityCost);
                command.Parameters.AddWithValue("p_restock_date", item.DateRestocked);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbConnection.transaction.Rollback();
                        dbConnection.transaction = null;
                        message = "Generating item " + item.ItemName + " restock report fail";
                        Console.WriteLine(message);
                        return null;
                    }

                    if (reader.Read())
                    {
                        restock = MapItemRestock(reader);
                    }

                    reader.Close();
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = "Generating item " + item.ItemName + " restock report success";
                    return restock;
                }
            }
        }
        catch (Exception e)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = e.Message;
            return new ItemRestock();
        }
    }


    private ItemRestock MapItemRestock(MySqlDataReader reader)
    {
        return new ItemRestock()
        {
            ItemId = reader.GetInt32("item_id"),
            ItemName = reader.GetString("item_name"),
            TotalCost = reader.GetDouble("total_cost"),
            RestockAmount = reader.GetInt32("restock_amount"),
            LiabilityPrice = reader.GetDouble("liability_cost"),
            RestockDate = reader.GetDateTime("restock_date"),
            IsPaid = reader.GetBoolean("has_paid"),
        };
    }
}