namespace CarlosYulo.backend.entities;

public class ItemRestock
{
    public int? ItemId { get; set; }
    public string? ItemName { get; set; }
    public double? TotalCost { get; set; }
    public int? RestockAmount { get; set; }
    public double? LiabilityPrice { get; set; }
    public DateTime RestockDate { get; set; }
    public bool IsPaid { get; set; }
    
    
    public override string ToString()
    {
        return $"ItemId: {ItemId?.ToString() ?? "N/A"}, " +
               $"ItemName: {ItemName}, " +
               $"RestockAmount: {RestockAmount?.ToString() ?? "N/A"}, " +
               $"LiabilityPrice: {LiabilityPrice?.ToString("C") ?? "N/A"}, " + // Format as currency
               $"RestockDate: {RestockDate.ToShortDateString()}, " +
               $"IsPaid: {IsPaid}";
    }
    
}