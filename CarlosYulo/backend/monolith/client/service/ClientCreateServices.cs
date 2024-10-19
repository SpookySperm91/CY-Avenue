using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.create;

namespace CarlosYulo.backend.monolith.client.c_create;

public class ClientCreateServices
{
    private readonly ClientCreateMember _createMember;
    private readonly ClientCreateWalkIn _createWalkIn;
    private readonly ErrorMessageBox _messageBox;


    public ClientCreateServices(
        ClientCreateMember createMember,
        ClientCreateWalkIn createWalkIn)
    {
        _createMember = createMember;
        _createWalkIn = createWalkIn;
        _messageBox = new ErrorMessageBox();
    }

    public bool CreateNewMember(Client? client)
    {
        string message;
        if (!_createMember.CreateNewMember(client, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool CreateNewWalkIn(Client? client)
    {
        string message;
        if (!_createWalkIn.CreateNewWalkIn(client, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
}