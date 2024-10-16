using System.Diagnostics;
using CarlosYulo.backend;
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
using CarlosYulo.backend.monolith.systemAccount.sy_login;
using CarlosYulo.database;

namespace CarlosYulo;

public partial class Form1 : Form
{
    // WARNING, WARNING, WARNING
    // BELOW AND BEYOND ARE TEST AND NOT SUBJECT TO FINAL PRODUCT
    // Testing all concrete and service class
    SystemAccountSearchByEmail _systemAccountSearchByEmail;
    SystemAccountSearchAll _systemAccountSearchAll;

    SystemAccountCheckAccount systemAccountCheckAccount;
    SystemAccountUpdatePassword _systemAccountUpdatePassword;
    SystemAccountUpdateDetails systemAccountUpdateDetails;
    SystemAccountVerification _systemAccountVerification;
    SystemAccountCompareVerification systemAccountCompareVerification;
    private SystemAccountVerificationDelete systemAccountVerificationDelete;

    public Form1(SystemAccountSearchByEmail systemAccountSearchByEmail,
        SystemAccountSearchAll systemAccountSearchAll, SystemAccountCheckAccount systemAccountCheckAccount,
        SystemAccountUpdatePassword systemAccountUpdatePassword, SystemAccountUpdateDetails systemAccountUpdateDetails,
        SystemAccountVerification systemAccountVerification,
        SystemAccountCompareVerification systemAccountCompareVerification,
        SystemAccountVerificationDelete systemAccountVerificationDelete)
    {
        InitializeComponent();
        _systemAccountSearchByEmail = systemAccountSearchByEmail;
        _systemAccountSearchAll = systemAccountSearchAll;

        this.systemAccountCheckAccount = systemAccountCheckAccount;
        this._systemAccountUpdatePassword = systemAccountUpdatePassword;
        this.systemAccountUpdateDetails = systemAccountUpdateDetails;
        _systemAccountVerification = systemAccountVerification;
        this.systemAccountCompareVerification = systemAccountCompareVerification;
        this.systemAccountVerificationDelete = systemAccountVerificationDelete;
    }


    private void btnSearchFullName_Click(object sender, EventArgs e)
    {
        // Client? client = clientService.SearchClientByFullName(txtbxFullName.Text, null);
        // try
        // {
        //     if (client != null)
        //     {
        //         lblName.Text = client.FullName;
        //         lblMemberShipId.Text = client.MembershipId.ToString();
        //         lblEmail.Text = client.Email;
        //         lblPhoneNumber.Text = client.PhoneNumber;
        //         lblGender.Text = client.Gender;
        //         lblAge.Text = client.Age.ToString();
        //         lblMemberShipStart.Text = client.BirthDate?.ToString("MM/dd/yyyy");
        //         // lblMembershipEnd.Text = Employee.MembershipEnd?.ToString("MM/dd/yyyy");
        //         return;
        //     }

        // catch (Exception exception)
        // {
        //     MessageBox.Show(exception.Message);
        //     Console.WriteLine(exception);
        //     throw;
        // }
    }


    // private Client? client;

    private SystemVerification _verification = null;
    private SystemAccount _systemAccount = null;

    private void btnSearchMemberShipId_Click(object sender, EventArgs e)
    {
        string message;
        _systemAccount =
            _systemAccountSearchByEmail.SearchByEmail("jhonandriecanedo1@gmail.com", out message);

        if (_systemAccount != null)
        {
            if (!_systemAccountVerification.GenerateSystemAccountVerificationAndSave(_systemAccount, out _verification,
                    out message))
            {
                MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine(_systemAccount.ToString());
            return;
        }

        MessageBox.Show(message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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