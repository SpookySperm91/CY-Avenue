using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop;

public class ItemUpdateDetails
{
    private DatabaseConnection dbConnection;

    public ItemUpdateDetails(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    public bool UpdateDetails(Item? item, out string message)
    {
        if (item == null)
        {
            Console.WriteLine("Item object cannot be null.");
            message = "Item object cannot be null.";
            return false;
        }
        
        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            // encase with 'using' to ensure proper dispose
            using (MySqlCommand command = new MySqlCommand("prcItemUpdateDetails", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateItemDataMap(command, item);

                // Execute the command
                command.ExecuteNonQuery();
                {
                    dbConnection.transaction.Commit(); // commit if update is successful
                    dbConnection.transaction = null; // always null transaction after query
                    Console.WriteLine("Item updated successfully.");
                    message = "Item data updated successfully.";
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            // Roll back if error occurs
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


    private void UpdateItemDataMap(MySqlCommand command, Item item)
    {
        command.Parameters.AddWithValue("p_item_id", item.ItemId);
        command.Parameters.AddWithValue("p_item_name", item.ItemName.TrimEnd());
        command.Parameters.AddWithValue("p_description", item.ItemDescription.TrimEnd());
        command.Parameters.AddWithValue("p_category", item.ItemCategory.TrimEnd());
        command.Parameters.AddWithValue("p_price", item.ItemPrice);
        command.Parameters.AddWithValue("p_liability_cost", item.ItemLiabilityCost);
        command.Parameters.AddWithValue("p_profile_pic", item.ItemPictureBytes);
    }
}