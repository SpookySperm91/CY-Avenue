using CarlosYulo.backend.monolith.client.c_create;
using CarlosYulo.backend.monolith.common;

namespace CarlosYulo.backend.monolith.client;

public class ClientUpdateServices
{
    private readonly ClientUpdateDetails _updateDetails;
    private readonly ClientUpdateMembership _updateMembership;
    private readonly ClientUpdateProfilePicture _updateProfilePicture;
    private readonly ClientUpdateStatus _updateStatus;
    private readonly ErrorMessageBox _messageBox;

    public ClientUpdateServices(
        ClientUpdateDetails updateDetails,
        ClientUpdateMembership updateMembership,
        ClientUpdateProfilePicture updateProfilePicture,
        ClientUpdateStatus updateStatus)
    {
        _updateDetails = updateDetails;
        _updateMembership = updateMembership;
        _updateProfilePicture = updateProfilePicture;
        _updateStatus = updateStatus;
        _messageBox = new ErrorMessageBox();
    }

    public bool UpdateDetails(Client client)
    {
        string message;
        if (!_updateDetails.UpdateDetails(client, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }


    public bool UpdateMembershipType(Client client, MembershipType membershipType)
    {
        string message;
        if (!_updateMembership.UpdateClientMembershipType(client, membershipType, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool UpdateProfilePicture(Client client, string picturePath)
    {
        string message;
        if (!_updateProfilePicture.UpdateProfilePicture(client, picturePath, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public void UpdateAllMembershipsToExpired()
    {
        _updateStatus.UpdateAllMembershipsToExpired();
    }
}