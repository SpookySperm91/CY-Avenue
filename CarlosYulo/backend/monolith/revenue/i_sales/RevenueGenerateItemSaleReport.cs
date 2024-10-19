using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop.i_revenue;

public class RevenueGenerateItemSaleReport
{
    private DatabaseConnection dbConnection;

    public RevenueGenerateItemSaleReport(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public List<ItemSales>? GenerateItemSales(List<Item> itemSold, out string message)
    {
        var itemSales = new List<ItemSales>();

        try
        {
            // Begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            foreach (Item item in itemSold)
            {
                if (item.ItemPrice is null || item.QuantityToBuy is null)
                {
                    message = "Item price and/or quantity is null";
                    return null;
                }

                decimal sale = item.QuantityToBuy.GetValueOrDefault() * (decimal)item.ItemPrice.GetValueOrDefault();
                using (MySqlCommand command = new MySqlCommand("prcRevenueItemSaleCreate", dbConnection.mysqlConnection,
                           dbConnection.transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("p_item_id", item.ItemId);
                    command.Parameters.AddWithValue("p_item_name", item.ItemName);
                    command.Parameters.AddWithValue("p_sale", sale);
                    command.Parameters.AddWithValue("p_quantity_sold", item.QuantityToBuy);
                    command.Parameters.AddWithValue("p_price", item.ItemPrice);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if there are results
                        if (!reader.HasRows)
                        {
                            message = "No rows found associated with the item category.";
                            Console.WriteLine(message);
                            return null;
                        }

                        // Retrieve rows and set in object
                        while (reader.Read())
                        {
                            itemSales.Add(MapItemSales(reader));
                        }
                    }
                }
            }

            dbConnection.transaction.Commit();
            dbConnection.transaction = null;
            Console.WriteLine("Successfully bought items");
            message = "Items bought successfully";
            return itemSales;
        }
        catch (Exception e)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            message = "An error occurred: " + e.Message;
            Console.WriteLine("Error buying items: " + e.Message);
            return null;
        }
    }


    public ItemSales MapItemSales(MySqlDataReader reader)
    {
        return new ItemSales()
        {
            ItemSalesId = reader["item_id"] != DBNull.Value ? Convert.ToInt32(reader["item_id"]) : null,
            ItemName = reader["item_name"] != DBNull.Value ? reader["item_name"].ToString() : null,
            ItemTotalSales = reader["sale"] != DBNull.Value
                ? Convert.ToDecimal(reader["sale"])
                : null,
            ItemQuantitiesSold = reader["quantity_sold"] != DBNull.Value
                ? Convert.ToInt32(reader["quantity_sold"])
                : null,
            ItemPriceSold = reader["price"] != DBNull.Value
                ? Convert.ToDecimal(reader["price"])
                : null,
            ItemSaleDate = reader["sold_at"] != DBNull.Value
                ? Convert.ToDateTime(reader["sold_at"])
                : null,
            IsRecorded = reader["is_recorded"] != DBNull.Value ? Convert.ToBoolean(reader["is_recorded"]) : null
        };
    }
}