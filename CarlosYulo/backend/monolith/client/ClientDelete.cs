using CarlosYulo.database;

namespace CarlosYulo.backend.monolith;

public class ClientDelete : IClientDelete
{
    private DatabaseConnector dbConnector;

    public ClientDelete(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public bool DeleteClient(ClientMembership client)
    {
        return true;
    }

    public bool DeleteClientByMembershipId(int membershipId)
    {
        return true;
    }
}