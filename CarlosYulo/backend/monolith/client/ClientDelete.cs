using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientDelete : IDelete<Client>
{
    private DatabaseConnector dbConnector;

    public ClientDelete(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public bool Delete(Client client)
    {
        return DeleteClientFunction("prcClientDeleteByMembershipId", null, client);
    }

    public bool DeleteById(int? membershipId)
    {
        return DeleteClientFunction("prcClientDeleteByMembershipId", membershipId, null);
    }


    private bool DeleteClientFunction(string storedProcedure, int? membershipId, Client? client)
    {
        try
        {
            using (var connection = dbConnector.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (membershipId.HasValue)
                    {
                        command.Parameters.AddWithValue("p_membership_id", membershipId.Value);
                    }
                    else if (client != null)
                    {
                        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
                    }


                    // Add output parameter for rows deleted
                    MySqlParameter rowsDeletedParam = new MySqlParameter("p_rows_deleted", MySqlDbType.Int32);
                    rowsDeletedParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(rowsDeletedParam);

                    // Execute the procedure
                    command.ExecuteNonQuery();

                    // Retrieve the number of rows deleted
                    int rowsDeleted = Convert.ToInt32(rowsDeletedParam.Value);
                    return rowsDeleted > 0;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting client: {e.Message}");
            return false;
        }
    }
}