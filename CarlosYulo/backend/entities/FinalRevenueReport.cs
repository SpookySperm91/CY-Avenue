namespace CarlosYulo.backend.entities;

public class FinalRevenueReport
{
    public DateTime RevenueDate { get; set; }
    public double? OriginalRevenue { get; set; }
    public double? TotalLiability { get; set; }
    public double? FinalRevenue { get; set; }
    
    public override string ToString()
    {
        return $"RevenueDate: {RevenueDate.ToString("yyyy-MM-dd")}, " +
               $"OriginalRevenue: {OriginalRevenue?.ToString("F2") ?? "N/A"}, " +
               $"TotalLiability: {TotalLiability?.ToString("F2") ?? "N/A"}, " +
               $"FinalRevenue: {FinalRevenue?.ToString("F2") ?? "N/A"}";
    }
}