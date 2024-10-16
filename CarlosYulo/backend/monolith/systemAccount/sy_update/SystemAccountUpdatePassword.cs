using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.systemAccount.sy_login;

public class SystemAccountUpdatePassword
{
    private DatabaseConnection dbConnection;
    private PasswordHashing passwordHashing;

    public SystemAccountUpdatePassword(DatabaseConnection dbConnection, PasswordHashing passwordHashing)
    {
        this.dbConnection = dbConnection;
        this.passwordHashing = passwordHashing;
    }

    public bool ChangePasswordAsAdmin(SystemAccount account, string password, out string message)
    {
        if (password.Length <= 8)
        {
            message = "Password must be at least 8 characters long.";
            return false;
        }


        return ChangePasswordNow(account, password, out message);
    }

    public bool ChangePassword(SystemAccount account, string previousPassword, string newPassword, out string message)
    {
        if (!passwordHashing.VerifyPassword(account, previousPassword))
        {
            message = "Invalid password";
            return false;
        }

        if (newPassword.Length <= 8)
        {
            message = "Password must be at least 8 characters long.";
            return false;
        }
        
        return ChangePasswordNow(account, newPassword, out message);
    }


    private bool ChangePasswordNow(SystemAccount account, string password, out string message)
    {
        // Hash password now
        passwordHashing.GenerateHashedPassword(account, password);

        // START OPERATION
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcSystemAccountUpdatePassword",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("p_user_id", account.UserId);
                command.Parameters.AddWithValue("p_new_password", account.Password);
                command.Parameters.AddWithValue("p_password_salt", account.PasswordSalt);

                command.ExecuteNonQuery();

                dbConnection.transaction.Commit();
                dbConnection.transaction = null;
                message = $"System Account : {account.UserId} password successfully updated.";
                return true;
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