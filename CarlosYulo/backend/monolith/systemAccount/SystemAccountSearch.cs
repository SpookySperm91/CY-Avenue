using System.Data;
using CarlosYulo.backend.monolith.systemAccount;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountSearch : ISearch<SystemAccount, SystemAccountType?>, ISystemAccountSearch
{
    private DatabaseConnector dbConnector;

    public SystemAccountSearch(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }


    public SystemAccount? SearchByEmail(string email, SystemAccountType? systemAccountType, out string message)
    {
        return Search(null, null, email, systemAccountType, out message);
    }

    public SystemAccount? SearchById(int id, SystemAccountType? systemAccountType, out string message)
    {
        return Search(id, null, null, systemAccountType, out message);
    }

    public SystemAccount? SearchByFullName(string fullName, SystemAccountType? systemAccountType, out string message)
    {
        return Search(null, fullName, null, systemAccountType, out message);
    }


    private SystemAccount? Search(int? id, string? fullName, string? email, SystemAccountType? systemAccountType, out string message)
    {
        SystemAccount? account = null;

        try
        {
            using (var connection = dbConnector.CreateConnection())
            {
                connection.Open();

                using (var command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    string accountTypeFormatted = systemAccountType.ToString().Substring(0, 1).ToUpper() +
                                                  systemAccountType.ToString().Substring(1).ToLower();

                    if (id.HasValue)
                    {
                        command.CommandText = "prcSystemAccountSearchById";
                        command.Parameters.AddWithValue("@p_user_id", id.Value);
                        command.Parameters.AddWithValue("@p_access_type", accountTypeFormatted);
                    }
                    else if (!string.IsNullOrEmpty(email))
                    {
                        command.CommandText = "prcSystemAccountSearchByEmail";
                        command.Parameters.AddWithValue("@p_email", email);
                        command.Parameters.AddWithValue("@p_access_type", accountTypeFormatted);
                    }
                    else if (!string.IsNullOrEmpty(fullName) && systemAccountType.HasValue)
                    {
                        command.CommandText = "prcSystemAccountSearchByUsername";
                        command.Parameters.AddWithValue("@p_username", fullName);
                        command.Parameters.AddWithValue("@p_access_type", accountTypeFormatted);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid search criteria.");
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account = MapSystemAccount(reader);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        if (account == null)
        {
            // If client is null, show an appropriate message based on what's available
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                message = "System Account username: " + fullName + " cannot be found";
            }
            else if (!string.IsNullOrWhiteSpace(email))
            {
                message = "System Account email: " + email + " cannot be found";
            }
            else if (id.HasValue)
            {
                message = "System Account ID: " + id.Value + " cannot be found";
            }
            else
            {
                message = "System Account not found";
            }

            return account;
        }
        
        message = "System Account found";
        return account;
    }


    // Helper method to map data from the reader to SystemAccount object
    private SystemAccount MapSystemAccount(MySqlDataReader reader)
    {
        return new SystemAccount
        {
            UserId = reader["user_id"] != DBNull.Value ? Convert.ToInt32(reader["user_id"]) : (int?)null,
            UserName = reader["username"].ToString(),
            Email = reader["email"].ToString(),
            Password = reader["password"].ToString(),
            PasswordSalt = reader["password_salt"].ToString(),
            AccountType =
                (SystemAccountType)Enum.Parse(typeof(SystemAccountType), reader["access"].ToString(), true),
            Logged = Convert.ToBoolean(reader["logged"]),
            Locked = Convert.ToBoolean(reader["locked"])
        };
    }


    public List<SystemAccount> SearchAll()
    {
        return new List<SystemAccount>();
    }
}