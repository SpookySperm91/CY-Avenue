using CarlosYulo.database;

namespace CarlosYulo.backend.monolith;

public class ClientUpdate : IUpdate<Client>, IClientUpdate
{
    private DatabaseConnector dbConnector;

    public ClientUpdate(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }
    
    public bool UpdateProfilePicture(Client client, string image, out string message)
    {
        // Initialize message to an empty string
        message = string.Empty;

        if (client is null)
        {
            message = "Client is null.";
            return false;
        }

        try
        {
            client.SetProfilePicture(image);
            message = "Profile picture updated successfully.";
            return true;
        }
        catch (FileNotFoundException ex)
        {
            message = $"File not found: {ex.Message}";
            Console.WriteLine(message);
            return false;
        }
        catch (InvalidDataException ex)
        {
            message = $"Invalid image format: {ex.Message}";
            Console.WriteLine(message);
            return false;
        }
        catch (Exception ex)
        {
            message = $"An unexpected error occurred: {ex.Message}";
            Console.WriteLine(message);
            return false;
        }
    }

    public bool Update(Client client)
    {
        if (client == null)
        {
            Console.WriteLine("Client cannot be null.");
            return false;
        }

        return UpdateClient("UpdateClientStoredProcedure", client, null);    }


    public bool UpdateClientMembershipType(Client client, MembershipType membership)
    {
        // Validate client and membership objects
        if (client == null || membership == null)
        {
            Console.WriteLine("Client or membership cannot be null.");
            return false;
        }

        // Assuming you have a stored procedure that handles the membership update
        return UpdateClient("UpdateMembershipStoredProcedure", client, membership);
    }


    private bool UpdateClient(string storedProcedure, Client client, MembershipType? membership)
    {
        return true;
    }
}