using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.delete;

namespace CarlosYulo.backend.monolith.client;

public class ClientDeleteServices
{
    private readonly ClientDelete _clientDelete;
    private readonly ClientDeleteAllExpired _clientDeleteAllExpired;
    private readonly ErrorMessageBox _messageBox;


    public ClientDeleteServices(
        ClientDelete clientDelete,
        ClientDeleteAllExpired clientDeleteAllExpired)
    {
        _clientDelete = clientDelete;
        _clientDeleteAllExpired = clientDeleteAllExpired;
        _messageBox = new ErrorMessageBox();
    }

    public bool DeleteClient(Client? client)
    {
        string message;
        if (!_clientDelete.DeleteEntity(client, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public void DeleteAllExpired(ClientDeleteType type)
    {
        switch (type)
        {
            case ClientDeleteType.MEMBERS:
                _clientDeleteAllExpired.DeleteMembershipAllExpired();
                break;

            case ClientDeleteType.WALK_IN:
                _clientDeleteAllExpired.DeleteWalkInAllExpired();
                break;

            default:
                throw new ArgumentException("Invalid client delete type.", nameof(type));
        }
    }
}

public enum ClientDeleteType
{
    MEMBERS,
    WALK_IN
}