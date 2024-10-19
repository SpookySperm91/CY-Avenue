using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith.employee;

public class SystemAccountEmail : EmailSendBase
{
    private string verificationCode;
    private string expirationTime;

    // Method to send the verification code email
    public bool SendVerificationCodeEmail(SystemVerification verification, string accountEmail, out string message)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(accountEmail) || string.IsNullOrWhiteSpace(verification.UserName))
            {
                message = "Please provide an email address and username";
                return false;
            }

            verificationCode = verification.Body;
            expirationTime = FormatExpirationTime(verification.ExpireAt);
            Subject = "System Account Verification";

            SendEmail(verification.UserName, accountEmail, verificationCode);
            message = "";
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            message = ex.Message;
            return false;
        }
    }

    // Method to load the email template
    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\",
            "EmailForgotPassword.html"));
    }

    // Override to replace placeholders in the email template
    protected override string SetEmailBody(string emailTemplate, string username, string? verificationCode)
    {
        return emailTemplate
            .Replace("<span id=\"username-placeholder\"></span>", username)
            .Replace("<div class=\"verification-code\" id=\"code-placeholder\">123456</div>",
                $"<div class=\"verification-code\">{verificationCode}</div>")
            .Replace("<span id=\"expire-time-placeholder\"></span>", expirationTime);
    }

    private string FormatExpirationTime(DateTime expireAt)
    {
        DateTime today = DateTime.Today;

        if (expireAt.Date == today)
        {
            return expireAt.ToString("h:mm tt") + " Today";
        }

        return expireAt.ToString("h:mm tt MMM dd");
    }
}