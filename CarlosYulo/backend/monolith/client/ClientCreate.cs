using CarlosYulo.database;

namespace CarlosYulo.backend.monolith;

public class ClientCreate : IClientCreate
{
    private DatabaseConnector dbConnector;

    public ClientCreate(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public bool CreateClient(ClientMembership client)
    {
        return true;
    }
}