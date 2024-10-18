using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemSearchById
{
    private DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;


    public ItemSearchById(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }


    public Item? SearchItemById(int itemId, out string message)
    {
        if (itemId.ToString().Length != 6)
        {
            message = "Invalid ID input. Please try again. Item ID must be 6 digits.";
            return null;
        }
        
        Item? item = null;
        try
        {
            using (MySqlCommand command =
                   new MySqlCommand("prcItemSearchById", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_item_id", itemId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)    
                    {
                        message = "Item search failed. Item-ID: " + itemId + " is not found";
                        Console.WriteLine("No row found associated with the item id: " + itemId);
                        return item;
                    }

                    if (reader.Read())
                    {
                        item = MapClient(reader);
                        message = "Item search successful";
                        return item;
                    }

                    message = "Error shit";
                    return item;
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine(message);
            return item;
        }
    }
    
    
    private Item MapClient(MySqlDataReader reader)
    {
        Item item = new Item
        {
            ItemId = Convert.ToInt32(reader["item_id"]),

            ItemName = reader["item_name"] != DBNull.Value ? reader["item_name"].ToString() : null,
            ItemDescription = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
            ItemCategory = reader["category"] != DBNull.Value ? reader["category"].ToString() : null,
            ItemPrice = reader["price"] != DBNull.Value ? Convert.ToDouble(reader["price"]) : null,
            ItemQuantity = reader["quantity"] != DBNull.Value ? Convert.ToInt32(reader["quantity"]) : null,
            ItemLiabilityCost = reader["liability_cost"] != DBNull.Value ? Convert.ToDouble(reader["liability_cost"]) : null,
            ItemPictureBytes = reader["picture"] != DBNull.Value ? (byte[])reader["picture"] : null
        };
        item.SetProfilePictureImage(null); // set as null default

        // Converge image byte from database into readable image
        if (item.ItemPictureBytes != null && item.ItemPictureBytes.Length > 0)
        {
            var image = imageViewer.ConvertByteArrayToImage(item.ItemPictureBytes);
            item.SetProfilePictureImage(image != null ? image : null); // Set image or null
        }

        return item;
    }
    
    
    
    
}