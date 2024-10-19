using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientUpdateDetails 
{
    private DatabaseConnection dbConnection;

    public ClientUpdateDetails(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool checkCondtions(Client client, out string errorMessage)
    {
        // Return false if client is null
        if (client == null)
        {
            Console.WriteLine("Client cannot be null.");
            errorMessage = "Client cannot be null.";
            return false;
        }

        errorMessage = "";
        return true;
    }


    public bool UpdateDetails(Client? client, out string message)
    {
        if (!checkCondtions(client, out message))
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

            // encase with 'using' to ensure proper dispose
            using (MySqlCommand command = new MySqlCommand("prcClientUpdateData", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateClientDataMap(command, client);

                // Execute the command
                command.ExecuteNonQuery();
                {
                    dbConnection.transaction.Commit(); // commit if update is successful
                    dbConnection.transaction = null; // always null transaction after query
                    Console.WriteLine("Updated client successfully.");
                    message = "Client data updated successfully.";
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


    private void UpdateClientDataMap(MySqlCommand command, Client client)
    {
        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_full_name", client.FullName?.TrimEnd());
        command.Parameters.AddWithValue("p_email", client.Email?.TrimEnd());
        command.Parameters.AddWithValue("p_phone_number", client.PhoneNumber?.TrimEnd());
        command.Parameters.AddWithValue("p_gender", client.Gender?.TrimEnd());
        command.Parameters.AddWithValue("p_age", client.Age ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_birthday", client.BirthDate?.Date ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_profile_pic", client.ProfilePictureByte);
    }
}