using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.delete;

public class ClientDelete 
{
    private DatabaseConnection dbConnection;

    public ClientDelete(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool IsValidMembershipId(int? membershipId)
    {
        return membershipId.HasValue && membershipId.Value >= 100000 && membershipId.Value <= 999999;
    }
    
    public bool DeleteEntity(Client? client, out string message)
    {
        if (client is null)
        {
            message = "Client object is null.";
            return false;
        }

        if (client.MembershipId == null)
        {
            message = "Client's Membership ID is null.";
            return false;
        }

        Console.WriteLine($"Deleting client with Membership ID: {client.MembershipId}");

        // Use the actual membership ID from the client object
        return DeleteOperation(client.MembershipId, out message);
    }


    public bool DeleteById(int membershipId, out string message)
    {
        if (membershipId < 100000 || membershipId > 999999)
        {
            message = "Invalid membership ID input. Please try again. Members ID must be 6 digits.";
            return false;
        }

        // helper class
        return DeleteOperation(membershipId, out message);
    }

    // helper class
    private bool DeleteOperation(int? membershipId, out string message)
    {
        if (!IsValidMembershipId(membershipId))
        {
            message = "Invalid membership ID input. Please try again. Members ID must be 6 digits.";
            return false;
        }

        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcClientDeleteByMembershipId",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_membership_id", membershipId);

                // Add output parameter for rows deleted
                MySqlParameter rowsDeletedParam = new MySqlParameter("p_rows_deleted", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rowsDeletedParam);

                // Execute the query
                command.ExecuteNonQuery();

                int rowsDeleted = Convert.ToInt32(rowsDeletedParam.Value);

                if (rowsDeleted > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"Client with Membership ID: {membershipId} successfully deleted.";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"No client found with Membership ID: {membershipId}. Deletion failed.";
                return false;
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
}