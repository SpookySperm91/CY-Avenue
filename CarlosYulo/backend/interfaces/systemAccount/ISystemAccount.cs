namespace CarlosYulo.backend.monolith.systemAccount;

public interface ISystemAccount
{
}

public interface ISystemAccountSearch
{
    SystemAccount? SearchByEmail(string email, SystemAccountType? systemAccountType, out string message);
}

public interface ISystemAccountForgotPassword
{
    void GeneratePasswordResetCode(SystemAccount systemAccount);
    void CheckVerification(SystemAccount systemAccount);
    void ChangePassword(SystemAccount systemAccount);
}