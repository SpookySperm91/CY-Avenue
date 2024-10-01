using CarlosYulo.database;

namespace CarlosYulo.backend.monolith;

public class ClientUpdate : IUpdate<Client>, IClientUpdate
{
    private DatabaseConnector dbConnector;

    public ClientUpdate(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public bool Update(Client client)
    {
        return true;
    }

    public bool UpdateProfilePicture(Client client, string image)
    {
        return true;
    }

    public bool UpdateClientMembershipType(Client client, MembershipType membership)
    {
        return true;
    }
}