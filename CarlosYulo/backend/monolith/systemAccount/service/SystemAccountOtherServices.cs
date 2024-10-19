using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee;

namespace CarlosYulo.backend.monolith.systemAccount;

public class SystemAccountOtherServices
{
    private readonly SystemAccountCreate _create;
    private readonly SystemAccountDelete _delete;
    private readonly SystemAccountEmail _email;
    private readonly ErrorMessageBox _messageBox;

    public SystemAccountOtherServices(
        SystemAccountCreate create,
        SystemAccountDelete delete,
        SystemAccountEmail email)
    {
        _create = create;
        _delete = delete;
        _email = email;
        _messageBox = new ErrorMessageBox();
    }

    public bool Create(SystemAccount systemAccount)
    {
        string message;
        if (!_create.Create(systemAccount, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool Delete(SystemAccount systemAccount)
    {
        string message;
        if (!_delete.Delete(systemAccount, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
    
    public bool DeleteById(int systemAccountId)
    {
        string message;
        if (!_delete.DeleteById(systemAccountId, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }


    public bool SendVerificationCodeEmail(SystemVerification verification, string accountEmail)
    {
        string message;
        if (!_email.SendVerificationCodeEmail(verification, accountEmail, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }
        return true;
    }


    
    
    
    
    
    
}