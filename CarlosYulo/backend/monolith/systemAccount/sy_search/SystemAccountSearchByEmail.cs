using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountSearchByEmail
{
    private DatabaseConnection dbConnection;

    public SystemAccountSearchByEmail(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public SystemAccount? SearchByEmail(string email, out string message)
    {
        if (email is null)
        {
            message = "Invalid email input. Please enter a valid email address.";
            return null;
        }


        SystemAccount systemAccount = new SystemAccount();

        try
        {
            using (MySqlCommand command = new MySqlCommand("prcSystemAccountSearchByEmail", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_email", email);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)
                    {
                        message = "System account search failed. Account email: " + email + " is not found";
                        Console.WriteLine("No row found associated with the account email: " + email);
                        return null;
                    }

                    if (reader.Read())
                    {
                        systemAccount = MapEmployee(reader);
                        message = "Employee search successful";
                        return systemAccount;
                    }

                    message = "Error shit";
                    message = string.Empty;
                    return null;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            message = e.Message;
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