namespace CarlosYulo.backend.monolith;

public class IClient {}


public interface IClientUpdate
{
    bool UpdateClientMembershipType(Client client, MembershipType membership);
}

public interface IClientGenerate
{
    int GenerateClientMembershipId(Client client);
}