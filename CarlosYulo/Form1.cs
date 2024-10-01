using CarlosYulo.backend;
using CarlosYulo.backend.monolith;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    IClientService clientService;

    public Form1(IClientService clientService)
    {
        InitializeComponent();
        this.clientService = clientService;
    }

    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        Client? client = clientService.SearchClientByFullName(txtbxFullName.Text, null);

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
        bool y = clientService.DeleteClientByMembershipId(Convert.ToInt32(txtbxMemberShipId.Text));
        if (y)
        {
            MessageBox.Show("Member" + txtbxMemberShipId.Text + " delete");
        }
        else
        {
            MessageBox.Show("Error deleting membership");
        }


        //     int membershipId;
        //     if (int.TryParse(txtbxMemberShipId.Text, out membershipId))
        //     {
        //         ClientMembership? client = clientService.SearchClientByMembershipId(membershipId, null);
        //         
        //         try
        //         {
        //             if (client != null)
        //             {
        //                 lblName.Text = client.FullName;
        //                 lblEmail.Text = client.Email;
        //                 lblPhoneNumber.Text = client.PhoneNumber;
        //                 lblGender.Text = client.Gender;
        //                 lblAge.Text = client.Age.ToString();
        //                 return;
        //             }
        //
        //             MessageBox.Show($"Client does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //             return;
        //         }
        //         catch (Exception exception)
        //         {
        //             MessageBox.Show(exception.Message);
        //             Console.WriteLine(exception);
        //             throw;
        //         }
        //     }
        //     MessageBox.Show($"Invalid input format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // }
    }
}