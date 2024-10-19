using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientUpdateMembership 
{
    private DatabaseConnection dbConnection;

    public ClientUpdateMembership(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool CheckConditions(Client? client, MembershipType? membershipType, out string message)
    {
        if (client is null)
        {
            message = "client is null";
            return false;
        }

        if (membershipType is null)
        {
            message = "membershipType is null, membershipType must be specified";
            return false;
        }

        if (membershipType == MembershipType.WALK_IN)
        {
            message = "Non members cannot update their membership type";
            return false;
        }

        message = string.Empty;
        return true;
    }


    public bool UpdateClientMembershipType(Client? client, MembershipType membershipType, out string message)
    {
        if (!CheckConditions(client, membershipType, out message))
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
            using (MySqlCommand command = new MySqlCommand("prcClientUpdateMembership", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateClientMembershipMap(command, client, membershipType);

                // Add OUT parameter for the membership result
                MySqlParameter outputMembershipParam = new MySqlParameter("p_membership", MySqlDbType.VarChar, 55)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputMembershipParam);

                // return true if success
                if (command.ExecuteNonQuery() > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null; // set to null after transaction

                    // instantiate the OUT response from the prc
                    client.SetMembership(outputMembershipParam.Value.ToString());

                    Console.WriteLine($"Updated client membership to '{client.Membership}' successfully.");
                    message = "Client data updated successfully.";
                    return true;
                }

                // Handle case where no rows were updated
                dbConnection.transaction.Rollback(); // Rollback the transaction if no rows were affected
                dbConnection.transaction = null;
                message = "No rows were updated.";
                return false;
            }
        }
        catch (Exception e)
        {
            // Roll back if error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null; // Rollback if no rows were affected
            }

            Console.WriteLine($"Error: {e.Message}");
            message = $"Failed to update client membership: {e.Message}";
            return false;
        }
    }

    private void UpdateClientMembershipMap(MySqlCommand command, Client client, MembershipType membershipType)
    {
        int membershipTypeId = membershipType switch
        {
            MembershipType.BASIC => 1,
            MembershipType.VIP => 2,
            _ => throw new ArgumentException("Unknown membership type.")
        };

        MembershipStatus status = MembershipStatus.ACTIVE;
        string statusString = status.ToString().Substring(0, 1).ToUpper() + status.ToString().Substring(1).ToLower();

        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_membership_type_id", membershipTypeId);
        command.Parameters.AddWithValue("p_membership_start", DateTime.Now);
        command.Parameters.AddWithValue("p_membership_end", DateTime.Now.AddDays(30));
        command.Parameters.AddWithValue("p_membership_status", statusString);
    }
}