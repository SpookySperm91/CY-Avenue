namespace CarlosYulo.backend.monolith;

public class IClient {}

public interface IClientCreate
{
    bool CreateWalkIn(Client client);
}

public interface IClientSearch
{
    Client? SearchWalkInByFullName(string fullName);
    Client? SearchWalkInByMembershipId(int membershipId);
}


public interface IClientUpdate
{
    bool UpdateClientMembershipType(Client client, MembershipType membership);
}

public interface IClientGenerate
{
    int GenerateClientMembershipId(Client client);
}