using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith;

public class ClientEmailExpire : EmailSendBase
{
    private string membershipType;
    private string expiryDate;
    

    public void SendMembershipExpiryEmail(Client client)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(client.Email) || string.IsNullOrWhiteSpace(client.FullName)
                                                    || client.MembershipTypeId >= 3)  // non-members are invalid 
        {
            MessageBox.Show("Email, username, and membership type must not be null or empty or invalid.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        membershipType = client.Membership;
        expiryDate = client.MembershipEnd?.ToString("MMMM dd, yyyy"); // e.g., "August 03, 2024"
        Subject = "Membership Expiration Notification";

        // call SendEmail method from base class
        SendEmail(client.FullName, client.Email, null);
    }


    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\",
            "EmailMembershipExpiration.html"));
    }

    // Override to fill in the placeholders for the client email
    protected override string SetEmailBody(string emailTemplate, string username, string? body)
    {
        return emailTemplate
            .Replace("<span id=\"username-placeholder\"></span>", $"<span>{username}</span>")
            .Replace("<span id=\"membership-type-placeholder\"></span>", $"<span>{membershipType}</span>")
            .Replace("<span id=\"expiry-date-placeholder\"></span>", $"<span>{expiryDate}</span>");
    }
}