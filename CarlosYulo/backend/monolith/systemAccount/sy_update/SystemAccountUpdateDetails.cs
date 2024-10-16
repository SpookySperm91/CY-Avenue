using System;
using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee
{
    public class SystemAccountUpdateDetails
    {
        private readonly DatabaseConnection dbConnection;

        public SystemAccountUpdateDetails(DatabaseConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public bool UpdateDetails(SystemAccount account, out string message)
        {
            try
            {
                // Begin transaction only if no existing transaction
                if (dbConnection.transaction is null)
                {
                    dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
                }

                // Use 'using' to ensure proper disposal
                using (MySqlCommand command = new MySqlCommand("prcSystemAccountUpdateData",
                           dbConnection.mysqlConnection, dbConnection.transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Map account properties to command parameters
                    UpdateSystemAccountMap(command, account);

                    // Execute the command
                    command.ExecuteNonQuery();
                    {
                        dbConnection.transaction.Commit(); // Commit if update is successful
                        dbConnection.transaction = null; // Set transaction to null after the query
                        
                        account.AllowDelete = Convert.ToBoolean(command.Parameters["p_allow_delete"].Value);
                        Console.WriteLine("Account updated successfully.");
                        message = "System account data updated successfully.";
                        return true;
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
        }

        private void UpdateSystemAccountMap(MySqlCommand command, SystemAccount account)
        {
            command.Parameters.AddWithValue("p_user_id", account.UserId);
            command.Parameters.AddWithValue("p_user_name", account.UserName?.TrimEnd());
            command.Parameters.AddWithValue("p_email", account.Email?.TrimEnd());
            command.Parameters.AddWithValue("p_access_type", account.AccessType);

            // Convert AllowDelete to bool safely
            bool allowDeleteValue =
                account.AllowDelete.HasValue && account.AllowDelete.Value; // Ensure it defaults correctly
            command.Parameters.AddWithValue("p_allow_delete", allowDeleteValue);

            // Set p_allow_delete as INOUT
            command.Parameters["p_allow_delete"].Direction = ParameterDirection.InputOutput;
        }
    }
}