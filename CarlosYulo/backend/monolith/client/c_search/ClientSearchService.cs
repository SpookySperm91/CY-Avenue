namespace CarlosYulo.backend.monolith;

public class ClientSearchService : IClientSearchChild
{
    private readonly ClientSearchById _clientSearchById;
    private readonly ClientSearchByName _clientSearchByFullName; // Ensure you have this class implemented.
    private readonly ClientSearchAll _clientSearchAll;

    public ClientSearchService(
        ClientSearchById clientSearchById,
        ClientSearchByName clientSearchByFullName,
        ClientSearchAll clientSearchAll)
    {
        _clientSearchById = clientSearchById;
        _clientSearchByFullName = clientSearchByFullName;
        _clientSearchAll = clientSearchAll;
    }

    public Client? SearchById(int membershipId, string? variable, out string message)
    {
        return _clientSearchById.SearchById(membershipId, variable, out message);
    }

    public List<Client> SearchByFullName(string fullName, string? variable, out string message)
    {
        return _clientSearchByFullName.SearchByFullName(fullName, variable, out message);
    }

    public List<Client> SearchAll(string type)
    {
        return _clientSearchAll.SearchAll(type);
    }
}
