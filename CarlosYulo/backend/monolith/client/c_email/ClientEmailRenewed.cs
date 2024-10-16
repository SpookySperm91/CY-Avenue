using CarlosYulo.backend.monolith.common;
using System.IO;
using System.Windows.Forms;

namespace CarlosYulo.backend.monolith;

public class ClientEmailRenewed : EmailSendBase
{
    private string membershipType;
    private string expiryDate;

    public void SendRenewedMembership(Client client)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(client.Email) || string.IsNullOrWhiteSpace(client.FullName) ||
            string.IsNullOrWhiteSpace(client.Membership) || client.MembershipTypeId < 1 ||
            client.MembershipTypeId >= 3) // Valid membership types (1 and 2)
        {
            MessageBox.Show("Email, username, and valid membership type must not be null or empty.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        membershipType = client.Membership;
        expiryDate = client.MembershipEnd?.ToString("MMMM dd, yyyy"); // e.g., "August 03, 2024"
        Subject = "Your Membership Renewal with CY-Avenue"; // Adjust the subject line for renewed membership

        // Call SendEmail method from base class
        SendEmail(client.FullName, client.Email, null);
    }

    // OVERRIDES METHODS FROM THE PARENT CLASS
    protected override string LoadEmailTemplate()
    {
        return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"template\",
            "EmailRenewedMembership.html")); // Load the renewed membership email template
    }

    protected override string SetEmailBody(string emailTemplate, string username, string? body)
    {
        // Replace placeholders in the email template with actual data
        return emailTemplate
            .Replace("<span id=\"username-placeholder\"></span>", username)
            .Replace("<span id=\"membership-type-placeholder\"></span>", membershipType)
            .Replace("<span id=\"expiry-date-placeholder\"></span>", expiryDate);
    }
}