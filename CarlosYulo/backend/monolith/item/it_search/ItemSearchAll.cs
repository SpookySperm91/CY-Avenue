using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemSearchAll
{
    private DatabaseConnection dbConnection;
    private ImageViewer    imageViewer;

    public ItemSearchAll(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }


    public List<Item>? SearchAll(out string message)
    {
        if (dbConnection.transaction == null)
        {
            dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
        }

        var items = new List<Item>();
        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand("prcItemSearchAll", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // MySqlDataReader read response
                // store rows in list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        message = "Error shit";
                        Console.WriteLine("Error. No item wthell");
                        return items;
                    }

                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        items.Add(MapItem(reader));
                    }

                    Console.WriteLine("Item search complete");
                    message = "Item search successful";
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine($"Error: {message}");
        }
        
        return items;
    }
    
    private Item MapItem(MySqlDataReader reader)
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