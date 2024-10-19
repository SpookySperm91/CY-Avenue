using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientUpdateProfilePicture 
{
    private DatabaseConnection dbConnection;

    public ClientUpdateProfilePicture(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool CheckCondition(Client client, string picturePath, out string message)
    {
        if (client is null)
        {
            message = "Client is null";
            return false;
        }

        if (client.MembershipTypeId > 2)
        {
            message = "Non members cannot change profile picture";
            return false;
        }

        if (!File.Exists(picturePath))
        {
            message = "Picture file does not exist";
            return false;
        }

        message = string.Empty;
        return true;
    }


    public bool UpdateProfilePicture(Client client, string picturePath, out string message)
    {
        if (!CheckCondition(client, picturePath, out message))
        {
            return false;
        }

        // set image path into readable and save in object
        if (!client.SetProfilePicture(picturePath, out message))
        {
            return false;
        }

        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)

            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcClientUpdateProfilePicture",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateClientProfileMap(command, client);

                // execute query and set transaction to null after
                command.ExecuteNonQuery();
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    Console.WriteLine("Updated client successfully.");
                    message = "Client profile picture updated successfully.";
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            // Roll back if error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = e.Message;
            return false;
        }
    }


    private void UpdateClientProfileMap(MySqlCommand command, Client client)
    {
        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_profile_pic", client.ProfilePictureByte);
    }
}