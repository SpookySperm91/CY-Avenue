namespace CarlosYulo.backend.entities;

public class GeneralLiabilityReport
{
    public DateTime? IncurredAt { get; set; }
    public double? TotalSalaryCost { get; set; }
    public double? TotalItemRestockCost { get; set; }
    public double? TotalCost { get; set; }
    
    public override string ToString()
    {
        return $"Financial Report:\n" +
               $"- Incurred At: {(IncurredAt.HasValue ? IncurredAt.Value.ToString("yyyy-MM-dd") : "N/A")}\n" +
               $"- Total Salary Cost: {TotalSalaryCost?.ToString("C") ?? "N/A"}\n" +
               $"- Total Item Restock Cost: {TotalItemRestockCost?.ToString("C") ?? "N/A"}\n" +
               $"- Total Cost: {TotalCost?.ToString("C") ?? "N/A"}";
    }
}