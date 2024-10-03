using System.Net.Mail;

namespace CarlosYulo.backend.monolith;

public interface InterfaceCommon
{
}

public interface ISMTPServer
{
    void SentHtmlEmail(string username, string email, string body);
    SmtpClient SetupSmtpServer();
}