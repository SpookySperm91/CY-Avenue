namespace CarlosYulo.backend.monolith.client;

public class ClientEmailService
{
    private readonly ClientEmailWelcome _clientEmailWelcome;
    private readonly ClientEmailExpire _clientEmailExpire;
    private readonly ClientEmailRenewed _clientEmailRenewed;

    public ClientEmailService(
        ClientEmailWelcome clientEmailWelcome,
        ClientEmailExpire clientEmailExpire,
        ClientEmailRenewed clientEmailRenewed)
    {
        _clientEmailWelcome = clientEmailWelcome;
        _clientEmailExpire = clientEmailExpire;
        _clientEmailRenewed = clientEmailRenewed;
    }

    public void SendEmail(Client client, EmailType type)
    {
        switch (type)
        {
            case EmailType.WELCOME_NEW_MEMBER:
                _clientEmailWelcome.SendWelcomeMembership(client);
                return;
            case EmailType.RENEWED_MEMBER:
                _clientEmailRenewed.SendRenewedMembership(client);
                return;
            case EmailType.EXPIRATION_NOTIFICATION:
                _clientEmailExpire.SendMembershipExpiryEmail(client);
                return;
        }
    }
}

public enum EmailType
{
    WELCOME_NEW_MEMBER,
    RENEWED_MEMBER,
    EXPIRATION_NOTIFICATION,
}