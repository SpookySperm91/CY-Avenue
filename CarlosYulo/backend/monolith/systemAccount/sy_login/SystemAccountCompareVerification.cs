using System.Data;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.systemAccount.sy_login;

public class SystemAccountCompareVerification
{
    private DatabaseConnection dbConnection;

    public SystemAccountCompareVerification(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public bool CompareVerification(SystemVerification verification, SystemAccount account, string userInput,
        out string message)
    {
        if (verification is null)
        {
            return CompareVerificationInDatabase(account.UserId, userInput, out message);
        }

        if (verification.UserId != account.UserId)
        {
            message = "Verification doesn't match";
            return false;
        }

        if (verification.ExpireAt < DateTime.Now)
        {
            message = "Verification already expired";
            return false;
        }

        if (userInput.ToUpper().Trim().Equals(verification.Body))
        {
            message = "Verification code is matched!";
            return true;
        }

        message = "Verification doesn't match";
        return false;
    }

    private bool CompareVerificationInDatabase(int? accountId, string userInput, out string message)
    {
        try
        {
            // Begin transaction if not already in one
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcSystemVerificationSearchById",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Execute the stored procedure
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string verification = reader["body"].ToString();

                        if (userInput.ToUpper().Trim().Equals(verification))
                        {
                            dbConnection.transaction.Commit();
                            dbConnection.transaction = null;
                            message = "Verification code is matched!";
                            return true;
                        }

                        dbConnection.transaction.Rollback();
                        dbConnection.transaction = null;
                        message = "Verification code is not matched. Try again!";
                        return false;
                    }

                    dbConnection.transaction.Rollback();
                    dbConnection.transaction = null;
                    message = "Error shit!";
                    return false;
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
            message = e.Message;
            return false;
        }
        finally
        {
            // Ensure transaction is null if not set elsewhere
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Dispose();
                dbConnection.transaction = null;
            }
        }
    }
}