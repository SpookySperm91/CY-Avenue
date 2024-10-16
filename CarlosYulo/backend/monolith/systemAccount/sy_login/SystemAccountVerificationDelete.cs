using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.systemAccount.sy_login;

public class SystemAccountVerificationDelete
{
    private DatabaseConnection dbConnection;

    public SystemAccountVerificationDelete(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    
    public void DeleteVerificationById(int? userId, out string message)
    {
        Console.WriteLine(userId);
        try
        {
            // Begin transaction if not already in one
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            // Use a new command for the delete operation
            using (MySqlCommand command = new MySqlCommand("prcSystemVerificationDeleteById",
                       dbConnection.mysqlConnection, dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_user_id", userId);

                // Execute the stored procedure
                command.ExecuteNonQuery();
                {
                    dbConnection.transaction.Commit();
                    message = "Verification record deleted successfully.";
                }
            }
        }
        catch (Exception e)
        {
            // Roll back if an error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = "Error occurred while deleting verification record: " + e.Message;
        }
    }
}