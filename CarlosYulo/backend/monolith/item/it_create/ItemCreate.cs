using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemCreate
{
    private DatabaseConnection dbConnection;
    private ImageViewer imageViewer;

    public ItemCreate(DatabaseConnection dbConnection, ImageViewer imageViewer)
    {
        this.dbConnection = dbConnection;
        this.imageViewer = imageViewer;
    }

    public bool Create(Item item, out string message)
    {
        // if client is null
        if (item is null)
        {
            message = "Item object is null";
            return false;
        }

        List<string> missingFields = new List<string>();
        ValidateFields(item, missingFields);

        // Create error message
        if (missingFields.Count > 0)
        {
            message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
            return false;
        }

        // START OPERATION
        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcItemCreateNew", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // add output parameters
                MySqlParameter outputId = new MySqlParameter("p_item_id", MySqlDbType.Int32)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputId);

                // instantiate values to prc parameter
                NewClientDataMap(command, item);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    item.ItemId = Convert.ToInt32(outputId.Value);

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"New item {item.ItemName} created successfully";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = "Failed to create new item";
                return false;
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
            return false;
        }
    }

    private void NewClientDataMap(MySqlCommand command, Item item)
    {
        command.Parameters.AddWithValue("p_item_name", item.ItemName?.TrimEnd());
        command.Parameters.AddWithValue("p_description", item.ItemDescription?.TrimEnd());
        command.Parameters.AddWithValue("p_category", item.ItemCategory?.TrimEnd());
        command.Parameters.AddWithValue("p_price", item.ItemPrice);
        command.Parameters.AddWithValue("p_quantity", item.ItemQuantity);
        command.Parameters.AddWithValue("p_liability_cost", item.ItemLiabilityCost);
        command.Parameters.AddWithValue("p_picture", item.ItemPictureBytes);
    }


    // Method to validate basic fields
    private void ValidateFields(Item item, List<string> missingFields)
    {
        if (string.IsNullOrWhiteSpace(item.ItemName))
        {
            missingFields.Add("Item Name");
        }

        if (string.IsNullOrWhiteSpace(item.ItemDescription))
        {
            missingFields.Add("Description");
        }

        if (string.IsNullOrWhiteSpace(item.ItemCategory))
        {
            missingFields.Add("Category");
        }

        if (item.ItemPrice == null || item.ItemPrice <= 0)
        {
            missingFields.Add("Price");
        }
        
        if (item.ItemQuantity == null || item.ItemQuantity <= 0)
        {
            missingFields.Add("Quantity");
        }

        if (item.ItemLiabilityCost == null || item.ItemLiabilityCost <= 0)
        {
            missingFields.Add("Liability Cost");
        }
        
        if (item.ItemPictureBytes != null && item.ItemPictureBytes.Length > 0 &&
            !imageViewer.IsValidImageFormat(item.ItemPictureBytes))
        {
            missingFields.Add("Profile Picture (must be a valid PNG or JPEG)");
        }
    }
}