using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountSearchAll
{
    private DatabaseConnection dbConnection;

    public SystemAccountSearchAll(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public List<SystemAccount>? SearchAll()
    {
        var accounts = new List<SystemAccount>();

        try
        {
            // encase with 'using' to ensure proper dispose
            using (MySqlCommand command = new MySqlCommand("prcSystemAccountSearchAll", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Retrieve rows and set in object
                    while (reader.Read())
                    {
                        accounts.Add(MapEmployee(reader));
                    }

                    Console.WriteLine("Retrieving and adding multiple rows to the list successfully.");
                    return accounts;
                }
            }
        }
        catch (MySqlException sqlEx)
        {
            Console.WriteLine("MySqlException: " + sqlEx);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex);
            return null;
        }
    }
    
    private SystemAccount MapEmployee(MySqlDataReader reader)
    {
        return new SystemAccount()
        {
            UserId = reader["user_id"] != DBNull.Value ? Convert.ToInt32(reader["user_id"]) : (int?)null,
            UserName = reader["username"] != DBNull.Value ? reader["username"].ToString() : null,
            Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
            Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
            PasswordSalt = reader["password_salt"] != DBNull.Value ? reader["password_salt"].ToString() : null,
            AccessType =
                reader["access"] != DBNull.Value
                    ? reader["access"].ToString()
                    : null,
            Logged = reader["logged"] != DBNull.Value && Convert.ToBoolean(reader["logged"]),
            Locked = reader["locked"] != DBNull.Value && Convert.ToBoolean(reader["locked"]),
            AllowDelete = reader["allow_deletion"] != DBNull.Value && Convert.ToBoolean(reader["allow_deletion"]),
        };
    }
}