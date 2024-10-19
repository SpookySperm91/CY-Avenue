using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith.client;

public class ClientSearchServices
{
    private readonly ClientSearchAll _clientSearchAll;
    private readonly ClientSearchById _clientSearchById;
    private readonly ClientSearchByName _clientSearchByName;
    private readonly ErrorMessageBox _messageBox;


    public ClientSearchServices(
        ClientSearchAll clientSearchAll,
        ClientSearchByName clientSearchByName,
        ClientSearchById clientSearchById)
    {
        _clientSearchAll = clientSearchAll;
        _clientSearchById = clientSearchById;
        _clientSearchByName = clientSearchByName;
        _messageBox = new ErrorMessageBox();
    }

    public List<Client> SearchAll(string type)
    {
        return _clientSearchAll.SearchAll(type);
    }

    public Client? SearchById(int id)
    {
        string message;
        var clientFound = _clientSearchById.SearchById(id, null, out message);

        if (clientFound is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return clientFound;
    }

    public List<Client>? SearchByFullName(string fullName)
    {
        string message;
        var clients = _clientSearchByName.SearchByFullName(fullName, null, out message);
        if (clients is null || !clients.Any())
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return clients;
    }
}