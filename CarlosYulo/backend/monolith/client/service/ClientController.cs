using CarlosYulo.backend.monolith.client.c_create;
using CarlosYulo.backend.monolith.create;
using CarlosYulo.backend.monolith.delete;

namespace CarlosYulo.backend.monolith.client;

public class ClientController
{
    private readonly ClientCreateServices _create;
    private readonly ClientUpdateServices _update;
    private readonly ClientSearchServices _search;
    private readonly ClientDeleteServices _delete;
    private readonly ClientEmailService _email;

    public ClientController(
        ClientCreateServices clientCreate,
        ClientUpdateServices clientUpdate,
        ClientSearchServices clientSearch,
        ClientDeleteServices clientDelete,
        ClientEmailService clientEmail)
    {
        _create = clientCreate;
        _update = clientUpdate;
        _search = clientSearch;
        _delete = clientDelete;
        _email = clientEmail;
    }

    // CREATE CLASSES
    public bool CreateNewMember(Client client)
    {
        return _create.CreateNewMember(client);
    }

    public bool CreateNewWalkIn(Client client)
    {
        return _create.CreateNewWalkIn(client);
    }


    // UPDATE 
    public bool UpdateDetails(Client client)
    {
        return _update.UpdateDetails(client);
    }

    public bool UpdateMembershipType(Client client, MembershipType membershipType)
    {
        return _update.UpdateMembershipType(client, membershipType);
    }

    public bool UpdateProfilePicture(Client client, string imagePath)
    {
        return _update.UpdateProfilePicture(client, imagePath);
    }

    public void UpdateExpireMembersToExpired()
    {
        _update.UpdateAllMembershipsToExpired();
    }


    // SEARCH
    public Client? SearchById(int id)
    {
        return _search.SearchById(id);
    }

    public List<Client>? SearchByFullName(string fullName)
    {
        return _search.SearchByFullName(fullName);
    }
    
    public List<Client> SearchAll(string type)
    {
        return _search.SearchAll(type);
    }
    
    
    // DELETE
    public bool DeleteClient(Client? client)
    {
        return _delete.DeleteClient(client);
    }

    public void DeleteAllExpired(ClientDeleteType type)
    {
        _delete.DeleteAllExpired(type);
    }
    
    
    // EMAIL
    public void SendEmail(Client client, EmailType type)
    {
        _email.SendEmail(client, type);
    }
}