using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.systemAccount.sy_login;

namespace CarlosYulo.backend.monolith.systemAccount;

public class SystemAccountLoginServices
{
    private readonly SystemAccountCheckAccount _accountCheck;
    private readonly SystemAccountCompareVerification _compareVerification;
    private readonly SystemAccountVerification _verification;
    private readonly SystemAccountVerificationDelete _verificationDelete;
    private readonly ErrorMessageBox _messageBox;

    public SystemAccountLoginServices(
        SystemAccountCheckAccount accountCheck,
        SystemAccountCompareVerification compareVerification,
        SystemAccountVerification verification,
        SystemAccountVerificationDelete verificationDelete)
    {
        _accountCheck = accountCheck;
        _compareVerification = compareVerification;
        _verification = verification;
        _verificationDelete = verificationDelete;
        _messageBox = new ErrorMessageBox();
    }

    public bool CheckAccountIfStaff(SystemAccount systemAccount)
    {
        string message;
        if (!_accountCheck.CheckAccountIfStaff(systemAccount, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool CheckAccountIfAdmin(SystemAccount systemAccount)
    {
        string message;
        if (!_accountCheck.CheckAccountIfAdmin(systemAccount, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool CompareVerification(SystemVerification verification, SystemAccount account, string userInput)
    {
        string message;
        if (!_compareVerification.CompareVerification(verification, account, userInput, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool GenerateSystemAccountVerificationAndSave(SystemAccount? account,
        out SystemVerification systemVerification)
    {
        string message;
        if (!_verification.GenerateSystemAccountVerificationAndSave(account, out systemVerification, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
    
    public void DeleteVerificationById(int? userId)
    {
        string message;
        _verificationDelete.DeleteVerificationById(userId, out message);
        _messageBox.ShowSuccessMessage(message);
    }
}