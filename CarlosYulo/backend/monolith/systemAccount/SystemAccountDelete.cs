using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountDelete : IDelete<SystemAccount>
{
    private DatabaseConnector dbConnector;

    public SystemAccountDelete(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public bool Delete(SystemAccount systemAccount)
    {
        return DeleteSystemAccount("prcSystemAccountDeleteByUserId", null, systemAccount);
    }

    public bool DeleteById(int? id)
    {
        return DeleteSystemAccount("prcSystemAccountDeleteByUserId", id, null);
    }

    private bool DeleteSystemAccount(string storedProcedure, int? id, SystemAccount? systemAccount)
    {
        try
        {
            using (var connection = dbConnector.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set parameters based on input values
                    if (id.HasValue)
                    {
                        command.Parameters.AddWithValue("p_user_id", id.Value);
                        command.Parameters.AddWithValue("p_access_type", null); // Null if no access type provided
                    }
                    else if (systemAccount != null)
                    {
                        string accountTypeFormatted = systemAccount?.AccountType.ToString().Substring(0, 1).ToUpper() +
                                                      systemAccount?.AccountType.ToString().Substring(1).ToLower();

                        command.Parameters.AddWithValue("p_user_id", systemAccount?.UserId);
                        command.Parameters.AddWithValue("p_access_type", accountTypeFormatted); // Admin/Staff enum
                    }

                    // Add output parameter for status message
                    MySqlParameter messageParam = new MySqlParameter("p_message", MySqlDbType.Int32);
                    messageParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(messageParam);

                    // Execute the procedure
                    command.ExecuteNonQuery();

                    // Retrieve the output value indicating success or failure
                    int messageCode = Convert.ToInt32(messageParam.Value);
                    switch (messageCode)
                    {
                        case 0:
                            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return true; // Deletion was successful
                        case 1:
                            MessageBox.Show("Error: User not found.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return false; // User not found
                        case 2:
                            MessageBox.Show("Error: Deletion not allowed for this account.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false; // Deletion not allowed
                        default:
                            MessageBox.Show("Unknown error occurred.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return false; // Catch-all for unknown errors
                    }
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error deleting account: {e.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false; // Return false in case of an exception
        }
    }
}