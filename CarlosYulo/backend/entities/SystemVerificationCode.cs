namespace CarlosYulo.backend;

public class SystemVerification
{
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Body { get; set; } // i.e verification, etc...
    public string? SystemAccountType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public bool Status { get; set; }

    public SystemVerification()
    {
    }
    
    public override string ToString()
    {
        return $"User ID: {(UserId.HasValue ? UserId.Value.ToString() : "N/A")}, " +
               $"User Name: {UserName ?? "N/A"}, " +
               $"Body: {Body ?? "N/A"}, " +
               $"Account Type: {SystemAccountType}, " +
               $"Created At: {CreatedAt}, " +
               $"Expire At: {ExpireAt}, " +
               $"Status: {(Status ? "Active" : "Inactive")}";
    }
}