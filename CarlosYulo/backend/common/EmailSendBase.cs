using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using Google.Protobuf;

namespace CarlosYulo.backend.monolith.common;

public abstract class EmailSendBase
{
    protected readonly string SmtpHost = "smtp.gmail.com";
    protected readonly string SmtpEmail = "starrysan.oficial@gmail.com";
    protected readonly string SmtpEmailPassword = "uzwt rgmw lpzo nrbm";
    protected readonly int SmtpPort = 587;
    protected string Subject = "Email Notification";

    protected abstract string SetEmailBody(string emailTemplate, string username, string? body);
    protected abstract string LoadEmailTemplate();


    protected void SendEmail(string username, string email, string? body)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Username and email must not be null or empty.");
        }

        string emailTemplate = LoadEmailTemplate();  // Load specific template in the derived class
        string emailBody = SetEmailBody(emailTemplate, username, body);  // Fill in the template in derived class

        using (SmtpClient smtpClient = SetupSmtpServer())
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(SmtpEmail);
            mail.Subject = Subject;
            mail.Body = emailBody;
            mail.IsBodyHtml = true;
            mail.To.Add(email);

            try
            {
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
    

    // Common SMTP setup
    private SmtpClient SetupSmtpServer()
    {
        return new SmtpClient(SmtpHost)
        {
            Port = SmtpPort,
            Credentials = new NetworkCredential(SmtpEmail, SmtpEmailPassword),
            EnableSsl = true
        };
    }
}