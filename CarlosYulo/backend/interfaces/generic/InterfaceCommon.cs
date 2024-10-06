using System.Net.Mail;
using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith;

public interface InterfaceCommon
{
}

public interface ISMTPServer
{
    // void SentHtmlEmail(string username, string email, string body, EmailType emailType);
    SmtpClient SetupSmtpServer();
}