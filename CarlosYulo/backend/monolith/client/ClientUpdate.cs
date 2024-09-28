using CarlosYulo.database;

namespace CarlosYulo.backend.monolith;

public class ClientUpdate: IClientUpdate
{
    private DatabaseConnector dbConnector;

    public ClientUpdate(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }
    
    public bool UpdateClient(ClientMembership client)
    {
        return true;
    }

    public bool UpdateClientProfilePicture(ClientMembership client, string image)
    {
        return true;
    }

    public bool UpdateClientMembershipType(ClientMembership client, MembershipType membership)
    {
        return true;
    }
}