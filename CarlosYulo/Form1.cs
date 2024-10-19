
using CarlosYulo.backend.monolith.client;

using CarlosYulo.backend.monolith.employee;

using CarlosYulo.backend.monolith.item;

using CarlosYulo.backend.monolith.systemAccount;


namespace CarlosYulo;

public partial class Form1 : Form
{
    // WARNING, WARNING, WARNING
    // BELOW AND BEYOND ARE TEST AND NOT SUBJECT TO FINAL PRODUCT
    // Testing all concrete and service class
    
    private EmployeeController employeeController;
    private ClientController clientController;
    private ItemController itemController;
    private SystemAccountController systemAccountController;


    public Form1(
        EmployeeController employeeController,
        ClientController clientController,
        ItemController itemController,
        SystemAccountController systemAccountController)
    {
        InitializeComponent();
        this.employeeController = employeeController;
        this.clientController = clientController;
        this.systemAccountController = systemAccountController;
        this.itemController = itemController;

 
    }


    private void btnSearchFullName_Click(object sender, EventArgs e)
    {

    }


    private void btnSearchMemberShipId_Click(object sender, EventArgs e)
    {
   
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