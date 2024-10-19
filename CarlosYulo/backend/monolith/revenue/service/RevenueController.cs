using CarlosYulo.backend.entities;
using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.revenue;

public class RevenueController
{
    private readonly RevenueLiabilityServices _liability;
    private readonly RevenueSaleServices _sale;

    public RevenueController(
        RevenueLiabilityServices liability,
        RevenueSaleServices sale)
    {
        _liability = liability;
        _sale = sale;
    }

    // LIABILITY
    public GeneralLiabilityReport? GenerateLiabilityReport(DateTime date)
    {
        return _liability.GenerateLiabilityReport(date);
    }

    public List<EmployeeSalary>? GenerateEmployeeSalary(DateTime month)
    {
        return _liability.GenerateEmployeeSalary(month);
    }

    public ItemRestock? GenerateItemRestock(Item item, int? quantity)
    {
        return _liability.GenerateItemRestock(item, quantity);
    }


    // SALES
    public FinalRevenueReport? GenerateFinalRevenueReport(DateTime month)
    {
        return _sale.GenerateFinalRevenueReport(month);
    }

    public PartialRevenueReport? CreateGeneralRevenueReport(DateTime date)
    {
        return _sale.CreatePartialRevenueReport(date);
    }

    public List<ItemSales>? GenerateItemSales(List<Item> itemSold)
    {
        return _sale.GenerateItemSales(itemSold);
    }

    public MembershipSale? GenerateMembershipSales(Client client, MembershipSaleType membershipSaleType)
    {
        return _sale.GenerateMembershipSales(client, membershipSaleType);
    }
}