using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountCreate : ICreate<SystemAccount>
{
    private DatabaseConnector dbConnector;
    private PasswordHashing passwordHashing;

    public SystemAccountCreate(DatabaseConnector dbConnector, PasswordHashing passwordHashing)
    {
        this.dbConnector = dbConnector;
        this.passwordHashing = passwordHashing;
    }

    public bool Create(SystemAccount entity, out string message)
    {
        if (string.IsNullOrWhiteSpace(entity.UserName))
        {
            message = "Please enter a username";
            return false;
        }

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            message = "Email Cannot be Empty";
            return false;
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(entity.Password))
            {
                passwordHashing.GenerateHashedPassword(entity, entity.Password);
            }
            else
            {
                MessageBox.Show("Input password is empty: default password will be 'password'", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                entity.Password = "password";
                passwordHashing.GenerateHashedPassword(entity, entity.Password);
            }

            string accountTypeFormatted = entity.AccountType.ToString().Substring(0, 1).ToUpper() +
                                          entity.AccountType.ToString().Substring(1).ToLower();

            using (var connection = dbConnector.CreateConnection())
            {
                connection.Open();

                using (var command = new MySqlCommand("prcSystemAccountCreate", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("p_username", entity.UserName.TrimEnd());
                    command.Parameters.AddWithValue("p_password", entity.Password.TrimEnd());
                    command.Parameters.AddWithValue("p_password_salt", entity.PasswordSalt.TrimEnd());
                    command.Parameters.AddWithValue("p_email", entity.Email.TrimEnd());
                    command.Parameters.AddWithValue("p_access", accountTypeFormatted);
                    command.Parameters.AddWithValue("p_logged", false);
                    command.Parameters.AddWithValue("p_locked", false);


                    if (entity.AccountType == SystemAccountType.STAFF || entity.AllowDelete == null)
                    {
                        command.Parameters.AddWithValue("p_allow_delete", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("p_allow_delete", entity.AllowDelete);
                    }

                    // Output parameter for user ID
                    MySqlParameter userIdParam = new MySqlParameter("p_user_id", MySqlDbType.Int32);
                    userIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(userIdParam);

                    command.ExecuteNonQuery();
                    
                    message = "Account creation success. Account User-ID: " + Convert.ToInt32(userIdParam.Value);
                }
            }

            return true;
        }
        catch (MySqlException ex)
        {
            // Display the error message from the exception
            message = ex.Message;
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            message = ex.Message;
        }

        return false;
    }
}