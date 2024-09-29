using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    IClientService clientService;

    public Form1(DatabaseConnector db)
    {
        InitializeComponent();
        clientService = new ClientService(
            new ClientCreate(db),
            new ClientUpdate(db),
            new ClientDelete(db),
            new ClientSearch(db));
    }

    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        ClientMembership? client = clientService.SearchClientByFullName(txtbxFullName.Text, null);

        try
        {
            if (client != null)
            {
                lblName.Text = client.FullName;
                lblMemberShipId.Text = client.MembershipId.ToString();
                lblEmail.Text = client.Email;
                lblPhoneNumber.Text = client.PhoneNumber;
                lblGender.Text = client.Gender;
                lblAge.Text = client.Age.ToString();
                return;
            }

            MessageBox.Show($"Client does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        int membershipId;
        if (int.TryParse(txtbxMemberShipId.Text, out membershipId))
        {
            ClientMembership? client = clientService.SearchClientByMembershipId(membershipId, null);
            
            try
            {
                if (client != null)
                {
                    lblName.Text = client.FullName;
                    lblEmail.Text = client.Email;
                    lblPhoneNumber.Text = client.PhoneNumber;
                    lblGender.Text = client.Gender;
                    lblAge.Text = client.Age.ToString();
                    return;
                }

                MessageBox.Show($"Client does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Console.WriteLine(exception);
                throw;
            }
        }
        MessageBox.Show($"Invalid input format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}