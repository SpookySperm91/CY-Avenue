namespace CarlosYulo.backend.monolith;

public class IClient {}

public interface IClientSearchChild: ISearchById<Client, string>, ISearchByFullName<Client, string>,ISearchAll<Client, string?> 
{
    
}







public interface IClientCreate
{
    bool CreateWalkIn(Client client, out string message);
}

public interface IClientSearch
{
    List<Client?> SearchWalkInByFullName(string fullName, out string message);
    Client? SearchWalkInByMembershipId(int membershipId, out string message);
}


public interface IClientUpdate
{
    bool UpdateClientMembershipType(Client client, MembershipType membershipType, out string message);
    
    // void UpdateClientMembershipType(Client client, MembershipType membership, out string message);

}

public interface IClientGenerate
{
    int GenerateClientMembershipId(Client client);
}

public interface IClientEmail
{
    void SendMembershipExpiryEmail(Client client);
}