using CarlosYulo.backend.entities;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.shop;
using CarlosYulo.backend.monolith.shop.i_revenue;

namespace CarlosYulo.backend.monolith.revenue;

public class RevenueSaleServices
{
    private readonly RevenueGenerateFinalReport _finalReport;
    private readonly RevenueGeneratePartialReport _partialReport;
    private readonly RevenueGenerateItemSaleReport _itemSaleReport;
    private readonly RevenueGenerateMembershipSalesReport _membershipSalesReport;
    private readonly ErrorMessageBox _messageBox;

    public RevenueSaleServices(
        RevenueGenerateFinalReport finalReport,
        RevenueGeneratePartialReport partialReport,
        RevenueGenerateItemSaleReport itemSaleReport,
        RevenueGenerateMembershipSalesReport membershipSalesReport)
    {
        _finalReport = finalReport;
        _partialReport = partialReport;
        _itemSaleReport = itemSaleReport;
        _membershipSalesReport = membershipSalesReport;
        _messageBox = new ErrorMessageBox();
    }

    public FinalRevenueReport? GenerateFinalRevenueReport(DateTime month)
    {
        string message;
        var finalRevenueReport = _finalReport.GenerateFinalRevenueReport(month, out message);
        if (finalRevenueReport is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return finalRevenueReport;
    }


    public PartialRevenueReport? CreatePartialRevenueReport(DateTime date)
    {
        string message;
        var partialRevenueReport = _partialReport.CreatePartialRevenueReport(date, out message);
        if (partialRevenueReport is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return partialRevenueReport;
    }

    public List<ItemSales>? GenerateItemSales(List<Item> itemSold)
    {
        string message;
        var itemSalesReport = _itemSaleReport.GenerateItemSales(itemSold, out message);
        if (itemSalesReport is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return itemSalesReport;
    }

    public MembershipSale? GenerateMembershipSales(Client client, MembershipSaleType membershipSaleType)
    {
        string message;
        var membershipSaleReport = _membershipSalesReport.GetMembershipSales(client, membershipSaleType, out message);
        if (membershipSaleReport is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return membershipSaleReport;
    }
}