using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using Moq;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemSearchByCategory
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public ItemSearchByCategory(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }


    public List<Item>? SearchByCategory(ItemCategory category, out string message)
    {
        string categoryString = category switch
        {
            ItemCategory.EQUIPMENT => "Equipment",
            ItemCategory.MERCHANDISE => "Merchandise",
            ItemCategory.SUPPLEMENT => "Supplement",
            _ => "Equipment"
        };

        var items = new List<Item>();
        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand("prcItemSearchByCategory", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_category", categoryString);

                // MySqlDataReader read response
                // store rows in list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        message = "Item category search fail. " + categoryString + " not found";
                        Console.WriteLine("No rows found associate with the item category: " + categoryString);
                        return null;
                    }

                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        items.Add(MapClient(reader));
                    }

                    Console.WriteLine("Client search complete");
                    message = "Client search successful";
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine($"Error in {nameof(categoryString)}: {message}");
        }
        
        return items;
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
            ItemLiabilityCost = reader["liability_cost"] != DBNull.Value
                ? Convert.ToDouble(reader["liability_cost"])
                : null,
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