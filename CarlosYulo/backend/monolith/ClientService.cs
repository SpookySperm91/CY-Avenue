using MySqlX.XDevAPI;

namespace CarlosYulo.backend.monolith;

public class ClientService: IClientService
{
    private IClientCreate clientCreate { get; set; }
    private IClientUpdate clientUpdate { get; set; }
    private IClientDelete clientDelete { get; set; }
    private IClientSearch clientSearch { get; set; }

    public ClientService(IClientCreate clientCreate,
                         IClientUpdate clientUpdate,
                         IClientDelete clientDelete,
                         IClientSearch clientSearch)
    {
        this.clientCreate = clientCreate;
        this.clientUpdate = clientUpdate;
        this.clientDelete = clientDelete;
        this.clientSearch = clientSearch;
    }
    
    // CREATE FUNCTION
    public bool CreateClient(ClientMembership clientMembership)
    {
        return clientCreate.CreateClient(clientMembership);
    }

    
    // UPDATE FUNCTIONS
    public bool UpdateClient(ClientMembership clientMembership)
    {
        return clientUpdate.UpdateClient(clientMembership);
    }
    public bool UpdateClientProfilePicture(ClientMembership clientMembership, string picture)
    {
        return clientUpdate.UpdateClientProfilePicture(clientMembership, picture);
    }
    public bool UpdateClientMembershipType(ClientMembership clientMembership, MembershipType membershipType)
    {
        return clientUpdate.UpdateClientMembershipType(clientMembership, membershipType);
    }
    
    
    // DELETE FUNCTIONS
    public bool DeleteClient(ClientMembership client)
    {
        return clientDelete.DeleteClient(client);
    }
    public bool DeleteClientByMembershipId(int membershipId)
    {
        return clientDelete.DeleteClientByMembershipId(membershipId);
    }
    
    
    // SEARCH FUNCTIONS
    public ClientMembership? SearchClientByMembershipId(int membershipId, string gender)
    {
        return clientSearch.SearchClientByMembershipId(membershipId, gender);
    }
    public ClientMembership? SearchClientByFullName(string fullName, string gender)
    {
        return clientSearch.SearchClientByFullName(fullName, gender);
    }
    
}