using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith;

public class ClientEmail : EmailSendBase, IClientEmail
{
    private string membershipType;
    private string expiryDate;

    public ClientEmail() { }

    public void SendMembershipExpiryEmail(Client client)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(client.Email) || string.IsNullOrWhiteSpace(client.FullName) 
                                                    || client.MembershipTypeId >=3 & client.MembershipTypeId < 1)
        {
            throw new ArgumentException("Email, username, and membership type must not be null or empty.");
        }

        switch (client.MembershipTypeId)
        {
            case 1: membershipType = "Accessibility"; break;
            case 2: membershipType = "VIP"; break;
        }
        
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