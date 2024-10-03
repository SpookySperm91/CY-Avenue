using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    ClientService clientService;
    EmailMessage email;
    
    
    public Form1(ClientService clientService, EmailMessage email)
    {
        InitializeComponent();
        this.clientService = clientService;
        this.email = email;
    }

    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        Client? client = clientService.SearchClientByFullName(txtbxFullName.Text, null);
        Console.WriteLine(txtbxFullName.Text);
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
                lblMemberShipStart.Text = client.MembershipStart.ToString("MM/dd/yyyy");
                lblMembershipEnd.Text = client.MembershipEnd.ToString("MM/dd/yyyy");
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
        try
        {
            email.SentHtmlEmail("Arima Kana", "jhonandriecanedo1@gmail.com", "This is a Test!");
            // MessageBox.Show($"An error occurred: {ex.Message}");

        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }
    
    private void txtbxFullName_TextChanged(object sender, EventArgs e)
    {

    }

}




      