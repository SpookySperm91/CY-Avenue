using MySqlX.XDevAPI;

namespace CarlosYulo.backend.monolith;

public class ClientService : IClientService
{
    private ICreate<Client> clientCreate { get; set; }
    private IUpdate<Client> clientUpdate { get; set; }
    private IDelete<Client> clientDelete { get; set; }
    private ISearch<Client> clientSearch { get; set; }
    private IClientUpdate iClientUpdate { get; set; }

    public ClientService(
        ICreate<Client> clientCreate,
        IUpdate<Client> clientUpdate,
        IDelete<Client> clientDelete,
        ISearch<Client> clientSearch,
        IClientUpdate iClientUpdate)
    {
        this.clientCreate = clientCreate;
        this.clientUpdate = clientUpdate;
        this.clientDelete = clientDelete;
        this.clientSearch = clientSearch;
        this.iClientUpdate = iClientUpdate;
    }

    // CREATE FUNCTION
    public bool CreateClient(Client client)
    {
        return clientCreate.Create(client);
    }


    // UPDATE FUNCTIONS
    public bool UpdateClient(Client client)
    {
        return clientUpdate.Update(client);
    }

    public bool UpdateClientProfilePicture(Client client, string picture)
    {
        return clientUpdate.UpdateProfilePicture(client, picture);
    }

    public bool UpdateClientMembershipType(Client client, MembershipType membershipType)
    {
        return iClientUpdate.UpdateClientMembershipType(client, membershipType);
    }


    // DELETE FUNCTIONS
    public bool DeleteClient(Client client)
    {
        return clientDelete.Delete(client);
    }

    public bool DeleteClientByMembershipId(int membershipId)
    {
        if (membershipId < 100000 || membershipId > 999999)
        {
            return false; 
        }
        
        return clientDelete.DeleteById(membershipId);
    }


    // SEARCH FUNCTIONS
    public Client? SearchClientByMembershipId(int membershipId, string? gender)
    {
        if (membershipId < 100000 || membershipId > 999999)
        {
            return null; 
        }
        return clientSearch.SearchById(membershipId, gender);
    }

    public Client? SearchClientByFullName(string fullName, string? gender)
    {
        if (String.IsNullOrEmpty(fullName))
        {
            return null;
        }
        return clientSearch.SearchByFullName(fullName, gender);
    }
}