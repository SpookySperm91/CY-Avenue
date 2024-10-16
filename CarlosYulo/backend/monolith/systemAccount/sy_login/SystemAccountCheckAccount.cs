namespace CarlosYulo.backend.monolith.systemAccount.sy_login;

public class SystemAccountCheckAccount
{
    public bool CheckAccountIfStaff(SystemAccount systemAccount, out string message)
    {
        return CheckAccountAccess(systemAccount, "staff", out message);
    }

    public bool CheckAccountIfAdmin(SystemAccount systemAccount, out string message)
    {
        return CheckAccountAccess(systemAccount, "admin", out message);
    }

    private bool CheckAccountAccess(SystemAccount systemAccount, string requiredAccessType, out string message)
    {
        if (systemAccount == null)
        {
            message = "Account is null";
            return false;
        }

        // Normalize access type for comparison
        string normalizedAccessType = requiredAccessType.ToLower();
        string accountAccessType = systemAccount.AccessType.ToLower();

        // Check access type first
        if (accountAccessType != normalizedAccessType)
        {
            message = $"Account is not {normalizedAccessType}";
            return false;
        }

        // Check if the account is locked
        if (systemAccount.Locked)
        {
            message = "Account is locked";
            return false;
        }

        // If both checks pass
        message = "";
        return true;
    }
}