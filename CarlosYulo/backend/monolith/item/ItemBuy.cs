using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemBuy
{
    private DatabaseConnection dbConnection;

    public ItemBuy(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public bool BuyItem(List<Item> itemList, out string message)
    {
        List<string> errorMessages = new List<string>();

        foreach (var item in itemList)
        {
            if (item.ItemQuantity < item.QuantityToBuy)
            {
                errorMessages.Add(item.ItemName);
            }
        }

        if (errorMessages.Count > 0)
        {
            message = "The following items have insufficient stock: " + string.Join(", ", errorMessages) + ".";
            Console.WriteLine(message);
            return false;
        }

        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            foreach (Item item in itemList)
            {
                using (MySqlCommand command = new MySqlCommand("prcItemUpdateQuantity", dbConnection.mysqlConnection,
                           dbConnection.transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_item_id", item.ItemId);
                    command.Parameters.AddWithValue("p_amount", item.QuantityToBuy);
                    command.Parameters.AddWithValue("p_action", "decrease");
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            message = "Updating item " + item.ItemName + " quantity failed";
                            dbConnection.transaction.Rollback();
                            dbConnection.transaction = null;
                            return false;
                        }

                        // retrieve rows and set in object
                        if (reader.Read())
                        {
                            item.ItemQuantity = reader["quantity"] != DBNull.Value
                                ? Convert.ToInt32(reader["quantity"])
                                : 0;
                        }
                    }
                }
            }
            dbConnection.transaction.Commit();
            dbConnection.transaction = null;
            Console.WriteLine("Successfully buy items");
            message = "Item bought successful";
            return true;
        }

        catch (Exception e)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            message = "An error occurred: " + e.Message;
            Console.WriteLine("Error buying: " + e.Message);
            return false;
        }
    }
}