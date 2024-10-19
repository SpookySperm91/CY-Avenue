using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee;

namespace CarlosYulo.backend.monolith.systemAccount;

public class SystemAccountSearchServices
{
    private readonly SystemAccountSearchAll _searchAll;
    private readonly SystemAccountSearchByEmail _searchByEmail;
    private readonly SystemAccountSearchById _searchById;
    private readonly ErrorMessageBox _messageBox;

    public SystemAccountSearchServices(
        SystemAccountSearchAll searchAll,
        SystemAccountSearchByEmail searchByEmail,
        SystemAccountSearchById searchById)
    {
        _searchAll = searchAll;
        _searchByEmail = searchByEmail;
        _searchById = searchById;
        _messageBox = new ErrorMessageBox();
    }

    public List<SystemAccount>? SearchAll()
    {
        return _searchAll.SearchAll();
    }

    public SystemAccount? SearchByEmail(string email)
    {
        string message;
        var systemAccount = _searchByEmail.SearchByEmail(email, out message);
        if (systemAccount == null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return systemAccount;
    }

    public SystemAccount? SearchById(int userId)
    {
        string message;
        var systemAccount = _searchById.SearchById(userId, out message);
        if (systemAccount == null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return systemAccount;
    }
}