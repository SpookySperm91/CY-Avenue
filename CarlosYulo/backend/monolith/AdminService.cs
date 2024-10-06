namespace CarlosYulo.backend.monolith;

public class AdminService
{
    private ICreate<SystemAccount> createSystemAccount;

    public AdminService(ICreate<SystemAccount> createSystemAccount)
    {
        this.createSystemAccount = createSystemAccount;
    }
    
    public bool CreateSystemAccount(SystemAccount systemAccount)
    {
        string outputMessage;
        bool result = createSystemAccount.Create(systemAccount, out outputMessage);

        // Show the output message after the method call
        MessageBox.Show(outputMessage, result ? "Success" : "Error", 
            result ? MessageBoxButtons.OK : MessageBoxButtons.OK, 
            result ? MessageBoxIcon.Information : MessageBoxIcon.Error);

        return result; // Return the result from the Create method
    }

    public bool DeleteSystemAccount(SystemAccount systemAccount)
    {
        return false;
    }
}