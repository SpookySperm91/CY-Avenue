using System.Data;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.database;
using MySql.Data.MySqlClient;
using System.Linq;

namespace CarlosYulo.backend.monolith.systemAccount.sy_login;

public class SystemAccountVerification
{
    private DatabaseConnection dbConnection;
    private SystemAccountEmail systemAccountEmail;

    public SystemAccountVerification(DatabaseConnection dbConnection, SystemAccountEmail systemAccountEmail)
    {
        this.dbConnection = dbConnection;
        this.systemAccountEmail = systemAccountEmail;
    }

    private string GenerateVerificationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        // Generate a random 6-character code
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public bool GenerateSystemAccountVerificationAndSave(SystemAccount? account,
        out SystemVerification systemVerification,
        out string message)
    {
        if (account == null)
        {
            systemVerification = null;
            message = "Please enter a valid system account number";
            return false;
        }

        string verificationCode = GenerateVerificationCode();
        Console.WriteLine(verificationCode);

        SystemVerification verification = new SystemVerification()
        {
            UserId = account.UserId,
            UserName = account.UserName,
            Body = verificationCode, // verification code
            SystemAccountType = account.AccessType,
            CreatedAt = DateTime.Now,
            ExpireAt = DateTime.Now.AddMinutes(30),
            Status = true
        };

        return SaveVerificationCodeToDatabase(verification, account.Email, out systemVerification, out message);
    }

    private bool SaveVerificationCodeToDatabase(SystemVerification verification, string email,
        out SystemVerification systemVerification, out string message)
    {
        // START OPERATION
        try
        {
            // Begin transaction if not already in one
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcSystemVerificationCreate", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Set parameters for the stored procedure
                SetDataMap(command, verification, email);

                // Add output parameter for status
                var statusParameter = new MySqlParameter("p_status", MySqlDbType.Bit)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(statusParameter);

                // Execute the stored procedure
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    // Send verification code email
                    if (!systemAccountEmail.SendVerificationCodeEmail(verification, email, out message))
                    {
                        systemVerification = null;
                        return false;
                    }

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"Verification code for System Account: {verification.UserId} successfully saved.";
                    verification.Status = Convert.ToBoolean(statusParameter.Value);
                    systemVerification = verification;
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Failed to create verification for System Account: {verification.UserName}.";
                systemVerification = null;
                return false;
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
            systemVerification = null;
            return false;
        }
    }

    private void SetDataMap(MySqlCommand command, SystemVerification verification, string email)
    {
        command.Parameters.AddWithValue("p_user_id", verification.UserId);
        command.Parameters.AddWithValue("p_username", verification.UserName);
        command.Parameters.AddWithValue("p_account_type", verification.SystemAccountType);
        command.Parameters.AddWithValue("p_email", email);
        command.Parameters.AddWithValue("p_verification_type", "Verification Code");
        command.Parameters.AddWithValue("p_body", verification.Body);
        command.Parameters.AddWithValue("p_create_at", verification.CreatedAt);
        command.Parameters.AddWithValue("p_expire_at", verification.ExpireAt);
    }
}