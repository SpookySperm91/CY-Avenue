using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientSearch : IClientSearch
{
    private readonly DatabaseConnector dbConnector;

    public ClientSearch(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public ClientMembership? SearchClientByMembershipId(int membershipId, string? gender)
    {
        return SearchClient("prcClientSearchByMembershipId", membershipId, null, gender);
    }

    public ClientMembership? SearchClientByFullName(string fullName, string? gender)
    {
        return SearchClient("prcClientSearchByName", null, fullName, gender);
    }

    private ClientMembership? SearchClient(string storedProcedure, int? membershipId, string? fullName, string? gender)
    {
        ClientMembership? client = null;

        using (var connection = dbConnector.CreateConnection())
        {
            // Open the connection
            connection.Open();

            using (var command = new MySqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters based on search type
                if (membershipId.HasValue)
                {
                    command.Parameters.AddWithValue("p_membership_id", membershipId.Value);
                }
                else if (!string.IsNullOrEmpty(fullName))
                {
                    command.Parameters.AddWithValue("p_client_full_name", fullName);
                }

                // Gender parameter (common to both procedures)
                command.Parameters.AddWithValue("p_gender",
                    string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new ClientMembership();
                            client.FullName = reader["full_name"].ToString();
                            client.MembershipId = Convert.ToInt32(reader["membership_id"]);
                            client.Email = reader["email"].ToString();
                            client.PhoneNumber = reader["phone_number"].ToString();
                            client.Age = Convert.ToInt32(reader["age"]);
                            client.Gender = reader["gender"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching client: {ex.Message}");
                }
            }
        }

        return client;
    }
}