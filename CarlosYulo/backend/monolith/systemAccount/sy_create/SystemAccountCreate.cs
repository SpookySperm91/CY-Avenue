using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountCreate 
{
    private DatabaseConnection dbConnection;
    private PasswordHashing passwordHashing;

    public SystemAccountCreate(DatabaseConnection dbConnection, PasswordHashing passwordHashing)
    {
        this.passwordHashing = passwordHashing;
        this.dbConnection = dbConnection;
    }


    private bool CheckCondition(SystemAccount systemAccount, out string message)
    {
        if (string.IsNullOrWhiteSpace(systemAccount.UserName))
        {
            message = "Please enter a username";
            return false;
        }

        if (string.IsNullOrWhiteSpace(systemAccount.Email))
        {
            message = "Email cannot be empty";
            return false;
        }

        if (string.IsNullOrWhiteSpace(systemAccount.AccessType))
        {
            message = "Access type cannot be empty";
            return false;
        }

        message = string.Empty;
        return true;
    }


    private bool CheckPassword(SystemAccount systemAccount, out string message)
    {
        message = string.Empty;

        if (string.IsNullOrWhiteSpace(systemAccount.Password))
        {
            MessageBox.Show("Input password is empty: default password will be 'password'", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            systemAccount.Password = "password"; // Set default password
        }
        else if (systemAccount.Password.Length < 8)
        {
            message = "Password must be at least 8 characters long";
            return false;
        }

        // Generate the hashed password
        passwordHashing.GenerateHashedPassword(systemAccount, systemAccount.Password);
        return true;
    }


    public bool Create(SystemAccount systemAccount, out string message)
    {
        if (!CheckCondition(systemAccount, out message) || !CheckPassword(systemAccount, out message))
        {
            return false;
        }

        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcSystemAccountCreate", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Output parameter for user ID
                MySqlParameter userIdParam = new MySqlParameter("p_user_id", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(userIdParam);

                // INOUT parameter for access type
                MySqlParameter accessTypeParam = new MySqlParameter("p_access_type", MySqlDbType.VarChar, 55)
                {
                    Value = systemAccount.AccessType, // Initial value for INOUT parameter
                    Direction = ParameterDirection.InputOutput
                };
                command.Parameters.Add(accessTypeParam);

                // Map account details
                MapSystemAccount(command, systemAccount);

                // Execute query
                int row = command.ExecuteNonQuery();
                if (row > 0)
                {
                    systemAccount.UserId = Convert.ToInt32(userIdParam.Value);
                    systemAccount.AccessType = accessTypeParam.Value.ToString(); // Get updated access type

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = "Account creation success. Account User-ID: " + systemAccount.UserId;
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = "Failed to create new system account";
                return false;
            }
        }
        catch (Exception ex)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction?.Rollback();
                dbConnection.transaction = null;
            }

            message = ex.Message;
            return false;
        }
    }

    private void MapSystemAccount(MySqlCommand command, SystemAccount systemAccount)
    {
        command.Parameters.AddWithValue("p_username", systemAccount.UserName.TrimEnd());
        command.Parameters.AddWithValue("p_password", systemAccount.Password.TrimEnd());
        command.Parameters.AddWithValue("p_password_salt", systemAccount.PasswordSalt.TrimEnd());
        command.Parameters.AddWithValue("p_email", systemAccount.Email.TrimEnd());
        command.Parameters.AddWithValue("p_logged", false);
        command.Parameters.AddWithValue("p_locked", systemAccount.Locked);
        command.Parameters.AddWithValue("p_allow_delete", systemAccount.AllowDelete);
    }
}