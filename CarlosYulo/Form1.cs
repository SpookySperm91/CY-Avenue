using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    private DatabaseConnector db;
    IClientService clientService;

    public Form1()
    {
        InitializeComponent();
        db = new DatabaseConnector();

        clientService = new ClientService(
            new ClientCreate(db),
            new ClientUpdate(db),
            new ClientDelete(db),
            new ClientSearch(db));
    }

    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        string fullName = txtbxFullName.Text;
        clientService.SearchClientByFullName(fullName, null);
    }

    private void btnSearchMemberShipId_Click(object sender, EventArgs e)
    {
        int membershipId;
        if (int.TryParse(txtbxMemberShipId.Text, out membershipId)) 
        {
            ClientMembership? client = clientService.SearchClientByMembershipId(membershipId, null);

            if (client != null)
            {
                // Assign the FullName to the Label's Text property
                lblName.Text = client.FullName;
                lblEmail.Text = client.Email;
                lblPhoneNumber.Text = client.PhoneNumber;
                lblGender.Text = client.Gender;
                lblAge.Text = client.Age.ToString();
                lblMemberShipStart.Text = client.MembershipStart.ToString("MM/dd/yyyy"); 
                lblMembershipEnd.Text = client.MembershipEnd.ToString("MM/dd/yyyy");
                lblMembershipStatus.Text = client.MembershipStatus.ToString();


            }
            else
            {
                lblName.Text = "Client not found";
            }
        }
        else
        {
            lblName.Text = "Invalid membership ID";
        }



    }
}