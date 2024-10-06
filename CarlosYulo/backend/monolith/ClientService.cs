using MySqlX.XDevAPI;

namespace CarlosYulo.backend.monolith;

public class ClientService
{
    private ICreate<Client> ClientCreate { get; set; }
    private IUpdate<Client> ClientUpdate { get; set; }
    private IDelete<Client> ClientDelete { get; set; }
    private ISearch<Client, string> ClientSearch { get; set; }
    private IClientCreate IClientCreate { get; set; }
    private IClientUpdate IClientUpdate { get; set; }
    private IClientEmail ClientEmail { get; set; }

    public ClientService(
        ICreate<Client> clientCreate,
        IUpdate<Client> clientUpdate,
        IDelete<Client> clientDelete,
        ISearch<Client, String> clientSearch,
        IClientCreate iClientCreate,
        IClientUpdate iClientUpdate,
        IClientEmail clientEmail)
    {
        this.ClientCreate = clientCreate;
        this.ClientUpdate = clientUpdate;
        this.ClientDelete = clientDelete;
        this.ClientSearch = clientSearch;
        this.IClientCreate = iClientCreate;
        this.IClientUpdate = iClientUpdate;
        this.ClientEmail = clientEmail;
    }

    // CREATE FUNCTION ///////////////////////////////////
    public bool CreateClient(Client client) 
    {
        if (client.MembershipTypeId == 3 || client.MembershipTypeId == 4)
        {
            MessageBox.Show("Error creating account. Invalid membership type.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }
        
        string message;
        bool result = ClientCreate.Create(client, out message);

        if (result)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return result;
    }

    public bool CreateClientWalkIn(Client client)
    {
        if (client.MembershipTypeId == 1 || client.MembershipTypeId == 2)
        {
            MessageBox.Show("Error creating account. Invalid walk-in type.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }

        string message;
        bool result = IClientCreate.CreateWalkIn(client, out message);

        if (result)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return result;
    }


    // UPDATE FUNCTIONS ///////////////////////////////////
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
        return IClientUpdate.UpdateClientMembershipType(client, membershipType);
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
            MessageBox.Show("Error searching user. Input Should be 6 digit.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return false;
        }
        return ClientDelete.DeleteById(membershipId);
    }


    // SEARCH FUNCTIONS ///////////////////////////////////
    public Client? SearchClientByMembershipId(string membershipIdInput, string? gender)
    {
        if (!int.TryParse(membershipIdInput, out int membershipId))
        {
            MessageBox.Show("Error: Membership ID must be a numeric value.", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }

        if (membershipId < 100000 || membershipId > 999999)
        {
            MessageBox.Show("Error searching user. Input should be 6 digit.", "Error search", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }
        
        string message;
        Client? client = ClientSearch.SearchById(membershipId, gender, out message);

        if (client == null)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        return client;
    }

    public Client? SearchClientByFullName(string fullName, string? gender)
    {
        if (String.IsNullOrEmpty(fullName) || String.IsNullOrWhiteSpace(fullName))
        {
            MessageBox.Show("Error searching user. Input are null.", "Error search", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }

        string message;
        Client? client = ClientSearch.SearchByFullName(fullName.TrimEnd(), gender, out message);

        if (client == null)
        {
            MessageBox.Show(message, "Error search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        return client;
    }

    public List<Client> SearchAllMembersClient()
    {
        return ClientSearch.SearchAll();
    }
}