using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountDelete
{
    private DatabaseConnection dbConnection;

    public SystemAccountDelete( DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public bool Delete(SystemAccount systemAccount, out string message)
    {
        if (systemAccount.UserId.HasValue)
        {
            return DeleteSystemAccount(systemAccount.UserId.Value, out message);
        }

        message = "System Account does not exist";
        return false;
    }

    public bool DeleteById(int userId, out string message)
    {
        if (userId.ToString().Length != 6)
        {
            message = "User id is not valid. User id must contain 6 digits";
            return false;
        }
        
        return DeleteSystemAccount(userId, out message);
    }

    private bool DeleteSystemAccount(int userId, out string message)
    {
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcSystemAccountDeleteByUserId",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_user_id", userId);


                // Add output parameter for status message
                MySqlParameter messageParam = new MySqlParameter("p_message", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(messageParam);

                // Execute the procedure
                command.ExecuteNonQuery();

                // Retrieve the output value indicating success or failure
                int row = Convert.ToInt32(messageParam.Value);

                if (row > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"System Account with ID: {userId} successfully deleted.";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"No system account found with ID: {userId}. Deletion failed.";
                return false;
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
            return false; // Return false in case of an exception
        }
    }
}