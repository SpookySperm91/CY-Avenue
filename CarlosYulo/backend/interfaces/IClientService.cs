namespace CarlosYulo.backend.monolith;

public interface IClientService
{
    // CREATE FUNCTION
    bool CreateClient(ClientMembership clientMembership);
    
    // UPDATE FUNCTIONS
    bool UpdateClient(ClientMembership clientMembership);
    bool UpdateClientProfilePicture(ClientMembership clientMembership, string picture);
    bool UpdateClientMembershipType(ClientMembership clientMembership, MembershipType membershipType);
    
    // DELETE FUNCTIONS
    bool DeleteClient(ClientMembership client);
    bool DeleteClientByMembershipId(int membershipId);
    
    // SEARCH FUNCTIONS
    ClientMembership? SearchClientByMembershipId(int membershipId, string? gender);
    ClientMembership? SearchClientByFullName(string fullName, string? gender);
}