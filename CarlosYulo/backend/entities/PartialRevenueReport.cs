namespace CarlosYulo.backend.entities;

public class PartialRevenueReport
{
    public DateTime RevenueDate { get; set; }
    public double ItemSales { get; set; }
    public double MembershipSales { get; set; }
    public double TotalRevenue {get; set;}
    
    

    public override string ToString()
    {
        return $"Revenue Date: {RevenueDate.ToShortDateString()}\n" +
               $"Item Sales: {ItemSales:C2}\n" +
               $"Membership Sales: {MembershipSales:C2}\n" +
               $"Total Revenue: {TotalRevenue:C2}";
    }
}