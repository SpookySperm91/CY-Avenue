namespace CarlosYulo.backend;

public class SystemAccount
{
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PasswordSalt { get; set; }
    public string AccessType { get; set; }
    public bool Logged { get; set; }
    public bool Locked { get; set; }
    public bool? AllowDelete { get; set; }
    
    public SystemAccount() { }

    public override string ToString()
    {
        return $"User ID: {UserId?.ToString() ?? "N/A"}, " +
               $"User Name: {UserName ?? "N/A"}, " +
               $"Email: {Email ?? "N/A"}, " +
               $"Account Type: {AccessType}, " +
               $"Logged: {Logged}, " +
               $"Locked: {Locked}, " +
               $"AllowDelete: {AllowDelete}";
    }
}

public enum SystemAccountType
{
    ADMIN, STAFF
}