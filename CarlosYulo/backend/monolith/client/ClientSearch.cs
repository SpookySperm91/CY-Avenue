using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientSearch : IClientSearch
{
    private DatabaseConnector dbConnector;

    public ClientSearch(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public ClientMembership? SearchClientByMembershipId(int membershipId, string gender)
    {
        return SearchClient("prcClientSearchByMembershipId", membershipId, null, gender);
    }

    public ClientMembership? SearchClientByFullName(string fullName, string gender)
    {
        return SearchClient("prcClientSearchByName", null, fullName, gender);
    }

    
    private ClientMembership? SearchClient(string storedProcedure, int? membershipId, string fullName, string gender)
    {
        ClientMembership client = null;

        using (var command = new MySqlCommand(storedProcedure, dbConnector.mysqlConnection))
        {
            command.CommandType = CommandType.StoredProcedure;

            if (membershipId.HasValue)
            {
                command.Parameters.AddWithValue("p_membership_id", membershipId.Value);
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                command.Parameters.AddWithValue("p_client_full_name", fullName);
            }

            command.Parameters.AddWithValue("p_gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);

            try
            {
                dbConnector.mysqlConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        client = new ClientMembership()
                        {
                            MembershipId = reader.GetInt32("membership_id"),
                            ProfilePicture = reader.GetString("profile_pic"),
                            FullName = reader.GetString("full_name"),
                            MembershipType = reader.GetInt32("membership_type"),
                            Email = reader.GetString("email"),
                            PhoneNumber = reader.GetString("phone_number"),
                            Gender = reader.GetString("gender"),
                            Age = reader.GetInt32("age"),
                            BirthDate = reader.GetDateTime("birthday"),
                            MembershipStart = reader.GetDateTime("membership_start"),
                            MembershipEnd = reader.GetDateTime("membership_end"),
                            MembershipStatus = reader.GetString("membership_status")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., logging)
                Console.WriteLine($"Error fetching client: {ex.Message}");
            }
            finally
            {
                dbConnector.mysqlConnection.Close();
            }
        }

        return client;
    }
}