namespace CarlosYulo.backend.entities;

public class MembershipSale
{
    public int? member_id { get; set; }
    public string? member_name { get; set; }
    public string? membership_type {get; set;}
    public double? price  { get; set; }
    public string? status { get; set; }
    public DateTime? sold_at { get; set; }
    
    
    public override string ToString()
    {
        return $"Member ID: {member_id?.ToString() ?? "N/A"}, " +
               $"Member Name: {member_name ?? "N/A"}, " +
               $"Membership Type: {membership_type ?? "N/A"}, " +
               $"Price: {price?.ToString() ?? "N/A"}, " +
               $"Status: {status ?? "N/A"}, " +
               $"Sold At: {sold_at?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}";
    }
    
}


public enum MembershipSaleType
{
    RENEW,
    UPGRADE,
    NEW_MEMBER,
    WALK_IN
}