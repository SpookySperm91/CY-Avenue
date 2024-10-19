namespace CarlosYulo.backend.monolith.systemAccount;

public class SystemAccountController
{
    private readonly SystemAccountOtherServices _others;
    private readonly SystemAccountLoginServices _login;
    private readonly SystemAccountSearchServices _search;

    public SystemAccountController(
        SystemAccountOtherServices others,
        SystemAccountLoginServices login,
        SystemAccountSearchServices search)
    {
        _others = others;
        _login = login;
        _search = search;
    }

    // OTHERS (CREATE, DELETE, EMAIL)
    public bool Create(SystemAccount systemAccount)
    {
        return _others.Create(systemAccount);
    }

    public bool Delete(SystemAccount systemAccount)
    {
        return _others.Delete(systemAccount);
    }

    public bool DeleteById(int systemAccountId)
    {
        return _others.DeleteById(systemAccountId);
    }

    public bool SendVerificationCodeEmail(SystemVerification verification, string accountEmail)
    {
        return _others.SendVerificationCodeEmail(verification, accountEmail);
    }
    
    
    // LOGIN
    public bool CheckAccountIfStaff(SystemAccount systemAccount)
    {
        return _login.CheckAccountIfStaff(systemAccount);
    }

    public bool CheckAccountIfAdmin(SystemAccount systemAccount)
    {
        return _login.CheckAccountIfAdmin(systemAccount);
    }

    public bool CompareVerification(SystemVerification verification, SystemAccount account, string userInput)
    {
        return _login.CompareVerification(verification, account, userInput);
    }

    public bool GenerateSystemAccountVerificationAndSave(SystemAccount? account,
        out SystemVerification systemVerification)
    {
        return _login.GenerateSystemAccountVerificationAndSave(account, out systemVerification);
    }

    public void DeleteVerificationById(int? userId)
    {
        _login.DeleteVerificationById(userId);
    }
    
    
    // SEARCH
    public List<SystemAccount>? SearchAll()
    {
        return _search.SearchAll();
    }

    public SystemAccount? SearchByEmail(string email)
    {
        return _search.SearchByEmail(email);
    }

    public SystemAccount? SearchById(int userId)
    {
        return _search.SearchById(userId);
    }
    
}