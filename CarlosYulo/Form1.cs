using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    // WARNING, WARNING, WARNING
    // BELOW AND BEYOND ARE TEST AND NOT SUBJECT TO FINAL PRODUCT
    // Testing all concrete and service class
    //
    ClientService clientService;
    IClientEmail email;
    ClientSearch clientSearch;
    SystemAccountSearch systemAccountSearch;
    SystemAccountDelete systemAccountDelete;
    SystemAccountCreate systemAccountCreate;
    EmployeeService employeeService;

    public Form1(ClientService clientService, ClientEmail email, ClientSearch clientSearch,
        SystemAccountSearch systemAccountSearch, SystemAccountDelete systemAccountDelete,
        SystemAccountCreate systemAccountCreate, EmployeeService employeeService)
    {
        InitializeComponent();
        this.clientService = clientService;
        this.email = email;
        this.clientSearch = clientSearch;
        this.systemAccountSearch = systemAccountSearch;
        this.systemAccountDelete = systemAccountDelete;
        this.systemAccountCreate = systemAccountCreate;
        this.employeeService = employeeService;
    }

    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        Employee? employee = employeeService.SearchEmployeeByFullName(txtbxFullName.Text, null);
        try
        {
            if (employee != null)
            {
                lblName.Text = employee.EmployeeFullName;
                lblMemberShipId.Text = employee.EmployeeId.ToString();
                lblEmail.Text = employee.Email;
                lblPhoneNumber.Text = employee.PhoneNumber;
                lblGender.Text = employee.Gender;
                lblAge.Text = employee.Age.ToString();
                lblMemberShipStart.Text = employee.Birthday?.ToString("MM/dd/yyyy");
                // lblMembershipEnd.Text = Employee.MembershipEnd?.ToString("MM/dd/yyyy");
                return;
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
            Console.WriteLine(exception);
            throw;
        }
    }

    private void btnSearchMemberShipId_Click(object sender, EventArgs e)
    {
        // Client? client = clientService.SearchClientByFullName("John Doe", null);
        // List<Client> allClient = clientSearch.SearchAll();

        Client newClient = new Client()
        {
            MembershipTypeId = 1,
            FullName = "Akane Kurokawa",
            Email = "akane.kurokawa@oshino.ko",
            PhoneNumber = "08888888888",
            Gender = "Female",
            Age = 19,
            BirthDate = new DateTime(2005, 8, 3)
        };

        clientService.CreateClient(newClient);


        List<Client> clients = clientService.SearchAllMembersClient();
        foreach (Client client in clients)
        {
            Console.WriteLine(newClient.ToString());
        }
    }

    private void txtbxFullName_TextChanged(object sender, EventArgs e)
    {
    }
}