using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountSearchById
{
    private DatabaseConnection dbConnection;

    public SystemAccountSearchById(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    public SystemAccount? SearchById(int userId, out string message)
    {
        if (userId.ToString().Length != 6)
        {
            message = "Invalid User-ID input. Please try again. Members ID must be 6 digits.";
            return null;
        }


        SystemAccount systemAccount = new SystemAccount();

        try
        {
            using (MySqlCommand command = new MySqlCommand("prcSystemAccountSearchById", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_user_id", userId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)
                    {
                        message = "System account search failed. Account-ID: " + userId + " is not found";
                        Console.WriteLine("No row found associated with the account id: " + userId);
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
            message = string.Empty;
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