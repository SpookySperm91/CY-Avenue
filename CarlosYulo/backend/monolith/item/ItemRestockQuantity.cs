using System.Data;
using CarlosYulo.database;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemRestockQuantity
{
    private DatabaseConnection dbConnection;

    public ItemRestockQuantity(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    public bool AddQuantity(Item item, int? quantity, out string message)
    {
        if (item.ItemId is null || item.ItemId.ToString().Length != 6)
        {
            message = "Item Id is null or invalid. It must be 6 characters long.";
            return false;
        }

        if (quantity is null || quantity <= 0)
        {
            message = "Quantity is zero or null";
            return false;
        }


        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand("prcItemUpdateQuantity", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_item_id", item.ItemId);
                command.Parameters.AddWithValue("p_amount", quantity);
                command.Parameters.AddWithValue("p_action", "increase");

                // MySqlDataReader read response
                // store rows in list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        message = "Updating item " + item.ItemName + " quantity fail";
                        Console.WriteLine("Updating item " + item.ItemName + " quantity fail");
                        return false;
                    }

                    // retrieve rows and set in object
                    if (reader.Read())
                    {
                        item.ItemQuantity = reader["quantity"] != DBNull.Value
                            ? Convert.ToInt32(reader["quantity"])
                            : null;
                    }

                    item.DateRestocked = DateTime.Now;
                    Console.WriteLine("Successfully updating item " + item.ItemName + " quantity to " +
                                      item.ItemQuantity);
                    message = "Updating quantity successful";
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine("Error in updating " + item.ItemName + " quanity");
            return false;
        }
    }
}