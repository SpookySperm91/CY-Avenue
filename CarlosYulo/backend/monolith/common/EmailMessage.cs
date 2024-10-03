using System.Net;
using System.Net.Mail;
using Google.Protobuf;

namespace CarlosYulo.backend.monolith.common;

public class EmailMessage : ISMTPServer
{
    private readonly string SmtpHost;
    private readonly string SmtpEmail;
    private readonly string SmtpEmailPassword;
    private readonly int SmtpPort;

    public EmailMessage()
    {
        this.SmtpHost = "smtp.gmail.com";
        this.SmtpEmail = "starrysan.oficial@gmail.com";
        this.SmtpEmailPassword = "uzwt rgmw lpzo nrbm";
        this.SmtpPort = 587;
    }

    public void SentHtmlEmail(string username, string email, string body)
    {
        // Validate inputs
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(body))
        {
            throw new ArgumentException("Username, email, and body must not be null or empty.");
        }

        string htmlPath = @"C:\Users\Jhon Andrie\RiderProjects\CarlosYulo\CarlosYulo\backend\monolith\common\html\EmailVerificationCode.html";
        
        string htmlTemplate = File.ReadAllText(htmlPath);
        string emailBody = htmlTemplate
            .Replace("<span id=\"username-placeholder\"></span>", $"<span>{username}</span>")
            .Replace("<div class=\"verification-code\" id=\"code-placeholder\">123456</div>", $"<div class=\"verification-code\">{body}</div>");

        using (SmtpClient smtpClient = SetupSmtpServer())
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(SmtpEmail);
            mail.Subject = "Forgot Password Verification";
            mail.Body = emailBody; // HTML body
            mail.IsBodyHtml = true; // Set to true to send HTML content
            mail.To.Add(email);

            try
            {
                smtpClient.Send(mail);
                Console.WriteLine("HTML Email sent successfully.");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                // You can also log ex.StackTrace for more details
            }
        }
    }


    public SmtpClient SetupSmtpServer() // Return the SmtpClient
    {
        return new SmtpClient(SmtpHost)
        {
            Port = SmtpPort, // Common SMTP port with TLS
            Credentials = new NetworkCredential(SmtpEmail, SmtpEmailPassword),
            EnableSsl = true
        };
    }
}