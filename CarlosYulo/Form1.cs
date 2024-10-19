using System.Diagnostics;
using CarlosYulo.backend;
using CarlosYulo.backend.entities;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.create;
using CarlosYulo.backend.monolith.delete;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.backend.monolith.employee.attendance;
using CarlosYulo.backend.monolith.employee.create;
using CarlosYulo.backend.monolith.employee.delete;
using CarlosYulo.backend.monolith.employee.salary;
using CarlosYulo.backend.monolith.employee.search;
using CarlosYulo.backend.monolith.employee.update;
using CarlosYulo.backend.monolith.revenue.i_liability;
using CarlosYulo.backend.monolith.shop;
using CarlosYulo.backend.monolith.shop.i_revenue;
using CarlosYulo.backend.monolith.systemAccount.sy_login;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    // WARNING, WARNING, WARNING
    // BELOW AND BEYOND ARE TEST AND NOT SUBJECT TO FINAL PRODUCT
    // Testing all concrete and service class

    private ClientSearchById clientSearchById;
    private ItemSearchByCategory itemSearchByCategory;
    private ItemBuy itemBuy;
    private RevenueGenerateItemSaleReport _revenueGenerateItemSaleReport;
    private RevenueGenerateMembershipSalesReport _revenueGenerateMembershipSalesReport;
    private RevenueGeneratePartialReport _revenueGeneratePartialReport;
    private LiabilityEmployeeSalary liabilityEmployeeSalary;
    private LiabilityItemRestock liabilityItemRestock;
    private ItemRestockQuantity itemRestockQuantity;
    private ItemSearchById itemSearchById;
    private LiabilityTotalMonth liabilityTotalMonth;
    private RevenueGenerateFinalReport _revenueGenerateFinalReport;

    public Form1(ClientSearchById clientSearchById, ItemBuy itemBuy, ItemSearchByCategory itemSearchByCategory,
        RevenueGenerateItemSaleReport revenueGenerateItemSaleReport,
        RevenueGenerateMembershipSalesReport revenueGenerateMembershipSalesReport, RevenueGeneratePartialReport revenueGeneratePartialReport,
        LiabilityEmployeeSalary liabilityEmployeeSalary,
        LiabilityItemRestock liabilityItemRestock,
        ItemRestockQuantity itemRestockQuantity,
        ItemSearchById itemSearchById,
        LiabilityTotalMonth liabilityTotalMonth,
        RevenueGenerateFinalReport revenueGenerateFinalReport)
    {
        InitializeComponent();

        this.clientSearchById = clientSearchById;

        this.itemBuy = itemBuy;
        this.itemSearchByCategory = itemSearchByCategory;

        this._revenueGenerateItemSaleReport = revenueGenerateItemSaleReport;
        this._revenueGenerateMembershipSalesReport = revenueGenerateMembershipSalesReport;
        this._revenueGeneratePartialReport = revenueGeneratePartialReport;
        this.liabilityEmployeeSalary = liabilityEmployeeSalary;
        this.liabilityItemRestock = liabilityItemRestock;
        this.itemRestockQuantity = itemRestockQuantity;

        this.itemSearchById = itemSearchById;
        this.liabilityTotalMonth = liabilityTotalMonth;
        
        this._revenueGenerateFinalReport = revenueGenerateFinalReport;
    }


    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        string message;
        List<Item> itemsToBuy = itemSearchByCategory.SearchByCategory(ItemCategory.MERCHANDISE, out message);
        if (itemsToBuy == null || itemsToBuy.Count == 0)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        foreach (Item item in itemsToBuy)
        {
            Console.WriteLine(item.ToString());
            item.QuantityToBuy = 3; // Set quantity to buy
        }

        // Attempt to buy items
        if (!itemBuy.BuyItem(itemsToBuy, out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Generate item sales after the items are successfully bought
        List<ItemSales> salesList = _revenueGenerateItemSaleReport.GenerateItemSales(itemsToBuy, out message);
        if (salesList == null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show("Items bought and sales generated successfully!", "Success", MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        foreach (var sales in salesList)
        {
            Console.WriteLine(sales.ToString());
        }
    }


    private void btnSearchMemberShipId_Click(object sender, EventArgs e)
    {
        string message;

        Item item = itemSearchById.SearchItemById(534871, out message);
        if (item == null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        liabilityEmployeeSalary.GenerateEmployeeSalary(new DateTime(2024, 10, 1), out message);

        item.QuantityToBuy = 10;

        if (!itemRestockQuantity.AddQuantity(item, item.QuantityToBuy, out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ItemRestock restockReport = liabilityItemRestock.GenerateItemRestock(item, item.QuantityToBuy, out message);
        if (restockReport is null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Console.WriteLine(restockReport.ToString());
        
        GeneralLiabilityReport report = liabilityTotalMonth.GenerateLiabilityReport(new DateTime(2024, 10, 1), out message);
        if (report is null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Console.WriteLine(report.ToString());
        
        FinalRevenueReport finalReport = _revenueGenerateFinalReport.GenerateFinalRevenueReport(new DateTime(2024, 10, 1), out message);
        if (finalReport is null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        Console.WriteLine(finalReport.ToString());
    }


    private void txtbxFullName_TextChanged(object sender, EventArgs e)
    {
    }

    private void table1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void btnUpdateProfile_Click(object sender, EventArgs e)
    {
    }
}