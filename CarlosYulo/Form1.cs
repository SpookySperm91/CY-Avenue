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

    ClientService cl;

    // List of all client 
    private List<Client> clients;


    public Form1(ClientService clientService, ClientEmail email, ClientSearch clientSearch,
        SystemAccountSearch systemAccountSearch, SystemAccountDelete systemAccountDelete,
        SystemAccountCreate systemAccountCreate, EmployeeService employeeService, ClientService cl)
    {
        InitializeComponent();
        this.Load += new EventHandler(Form1_Load);
        this.table1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.table1_CellClick);

        this.clientService = clientService;
        this.email = email;
        this.clientSearch = clientSearch;
        this.systemAccountSearch = systemAccountSearch;
        this.systemAccountDelete = systemAccountDelete;
        this.systemAccountCreate = systemAccountCreate;
        this.employeeService = employeeService;

    }

    private void Form1_Load(object sender, EventArgs e)
    {
        clients = clientService.SearchAllMembersClient();

        // Ensure auto size for columns
        table1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Clear existing rows
        table1.Rows.Clear();

        // Add relevant columns
        table1.Columns.Add("MembershipId", "Membership ID");
        table1.Columns.Add("FullName", "Full Name");
        table1.Columns.Add("Membership", "Membership");
        table1.Columns.Add("Email", "Email");
        table1.Columns.Add("PhoneNumber", "Phone Number");

        table1.Columns.Add("MembershipStart", "Membership Start");
        table1.Columns.Add("MembershipEnd", "Membership End");
        table1.Columns.Add("MembershipStatus", "Membership Status");

        // Ensure auto size for columns
        table1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        // Clear existing rows
        table1.Rows.Clear();

        // Add rows to the DataGridView
        foreach (var client in clients)
        {
            table1.Rows.Add(
                client.MembershipId,
                client.FullName,
                client.Membership,
                client.Email,
                client.PhoneNumber,
                client.MembershipStart?.ToString("yyyy-MM-dd"),
                client.MembershipEnd?.ToString("yyyy-MM-dd"),
                client.MembershipStatus
            );
        }
    }

    private void table1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            int rowIndex = e.RowIndex;
            if (IsRowEmpty(rowIndex))
            {
                return;
            }

            Client selectedClient = clients[rowIndex];

            updateShit(selectedClient);
        }

        // Method to check if the specified row is empty
        bool IsRowEmpty(int rowIndex)
        {
            // Check if the cells in the row contain default values or are empty
            return string.IsNullOrWhiteSpace(table1.Rows[rowIndex].Cells["FullName"].Value?.ToString());
        }
    }

    private void updateShit(Client selectedClient)
    {
        lblMemberShipId.Text = selectedClient.MembershipId.ToString();
        lblFullName.Text = selectedClient.FullName;
        lblEmail.Text = selectedClient.Email;
        lblPhoneNumber.Text = selectedClient.PhoneNumber;
        lblGender.Text = selectedClient.Gender;
        lblBirthDay.Text = selectedClient.BirthDate?.ToString("yyyy-MM-dd");
        lblMemberShipStart.Text = selectedClient.MembershipStart?.ToString("yyyy-MM-dd");
        lblMembershipEnd.Text = selectedClient.MembershipEnd?.ToString("yyyy-MM-dd");
        lblMembershipStatus.Text = selectedClient.MembershipStatus;
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

    private void table1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }


}