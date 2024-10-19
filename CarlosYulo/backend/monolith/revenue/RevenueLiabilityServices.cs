using CarlosYulo.backend.entities;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.revenue.i_liability;
using CarlosYulo.backend.monolith.shop;

namespace CarlosYulo.backend.monolith.revenue;

public class RevenueLiabilityServices
{
    private readonly LiabilityTotalMonth _totalMonth;
    private readonly LiabilityEmployeeSalary _employeeSalary;
    private readonly LiabilityItemRestock _itemRestock;
    private readonly ErrorMessageBox _messageBox;

    public RevenueLiabilityServices(
        LiabilityTotalMonth totalMonth,
        LiabilityEmployeeSalary employeeSalary,
        LiabilityItemRestock itemRestock)
    {
        _totalMonth = totalMonth;
        _employeeSalary = employeeSalary;
        _itemRestock = itemRestock;
        _messageBox = new ErrorMessageBox();
    }

    public GeneralLiabilityReport? GenerateLiabilityReport(DateTime date)
    {
        string message;
        var liabilityReport = _totalMonth.GenerateLiabilityReport(date, out message);
        if (liabilityReport is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return liabilityReport;
    }

    public List<EmployeeSalary>? GenerateEmployeeSalary(DateTime month)
    {
        string message;
        var employeeSalary = _employeeSalary.GenerateEmployeeSalary(month, out message);
        if (employeeSalary is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return employeeSalary;
    }

    public ItemRestock? GenerateItemRestock(Item item, int? quantity)
    {
        string message;
        var restock = _itemRestock.GenerateItemRestock(item, quantity, out message);
        if (restock is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return restock;
    }
}