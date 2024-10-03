using MySqlX.XDevAPI;

namespace CarlosYulo.backend.monolith;

public class ClientService 
{
    private ICreate<Client> ClientCreate { get; set; }
    private IUpdate<Client> ClientUpdate { get; set; }
    private IDelete<Client> ClientDelete { get; set; }
    private ISearch<Client, string> ClientSearch { get; set; }
    private IClientCreate IClientCreate { get; set; }
    private IClientUpdate iClientUpdate { get; set; }

    public ClientService(
        ICreate<Client> clientCreate,
        IUpdate<Client> clientUpdate,
        IDelete<Client> clientDelete,
        ISearch<Client, String> clientSearch,
        IClientCreate iClientCreate,
        IClientUpdate iClientUpdate)
    {
        this.ClientCreate = clientCreate;
        this.ClientUpdate = clientUpdate;
        this.ClientDelete = clientDelete;
        this.ClientSearch = clientSearch;
        this.IClientCreate = iClientCreate;
        this.iClientUpdate = iClientUpdate;
    }

    // CREATE FUNCTION
    public bool CreateClient(Client client)
    {
        return ClientCreate.Create(client);
    }

    public bool CreateClientWalkIn(Client client)
    {
        if (client.MembershipType == 1 || client.MembershipType == 2 )
        {
            MessageBox.Show("Error creating account. Invalid membership type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        
        return IClientCreate.CreateWalkIn(client);
    }


    // UPDATE FUNCTIONS
    public bool UpdateClient(Client client)
    {
        return ClientUpdate.Update(client);
    }

    public bool UpdateClientProfilePicture(Client client, string picture)
    {
        return ClientUpdate.UpdateProfilePicture(client, picture);
    }

    public bool UpdateClientMembershipType(Client client, MembershipType membershipType)
    {
        return iClientUpdate.UpdateClientMembershipType(client, membershipType);
    }


    // DELETE FUNCTIONS
    public bool DeleteClient(Client client)
    {
        return ClientDelete.Delete(client);
    }

    public bool DeleteClientByMembershipId(int? membershipId)
    {   
        
        if (membershipId < 100000 || membershipId > 999999 || !membershipId.HasValue)
        {
            MessageBox.Show("Error searching user. Input Should be 6 digit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false; 
        }
        return ClientDelete.DeleteById(membershipId);
    }


    // SEARCH FUNCTIONS
    public Client? SearchClientByMembershipId(int membershipId, string? gender)
    {
        if (membershipId < 100000 || membershipId > 999999)
        {
            MessageBox.Show("Error searching user. Input should be 6 digit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null; 
        }
        return ClientSearch.SearchById(membershipId, gender);
    }

    public Client? SearchClientByFullName(string fullName, string? gender)
    {
        if (String.IsNullOrEmpty(fullName))
        {
            MessageBox.Show("Error searching user. Input are null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        return ClientSearch.SearchByFullName(fullName, gender);
    }
}