namespace CarlosYulo.backend.monolith;

public class IClient
{
    
}

public interface IClientCreate
{
    bool CreateClient(ClientMembership client);
}

public interface IClientSearch
{
    ClientMembership? SearchClientByMembershipId(int membershipId, string gender);
    ClientMembership? SearchClientByFullName(string fullName, string gender);
}

public interface IClientUpdate
{
    bool UpdateClient(ClientMembership client);
    bool UpdateClientProfilePicture(ClientMembership client, string image);
    bool UpdateClientMembershipType(ClientMembership client, MembershipType membership);
}

public interface IClientDelete
{
    bool DeleteClient(ClientMembership client);
    bool DeleteClientByMembershipId(int membershipId);
}

public interface IClientGenerate
{
    int GenerateClientMembershipId(ClientMembership client);
}