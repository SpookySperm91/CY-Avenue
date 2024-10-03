namespace CarlosYulo.backend.monolith;

public interface IClientService
{
    // CREATE FUNCTION
    bool CreateClient(Client client);
    
    // UPDATE FUNCTIONS
    bool UpdateClient(Client client);
    bool UpdateClientProfilePicture(Client client, string picture);
    bool UpdateClientMembershipType(Client client, MembershipType membershipType);
    
    // DELETE FUNCTIONS
    bool DeleteClient(Client client);
    bool DeleteClientByMembershipId(int membershipId);
    
    // SEARCH FUNCTIONS goods
    Client? SearchClientByMembershipId(int membershipId, string? gender);
    Client? SearchClientByFullName(string fullName, string? gender);
}