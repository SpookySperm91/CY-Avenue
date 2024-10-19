using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientSearchByName
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public ClientSearchByName(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    public List<Client> SearchByFullName(string fullName, string? gender, out string message)
    {
        var clients = new List<Client>();

        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand("prcClientSearchByName", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_client_full_name", fullName.TrimEnd());
                command.Parameters.AddWithValue("p_gender", gender.TrimEnd());

                // MySqlDataReader read response
                // store rows in list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        message = "Client search fail. " + fullName + " not found";
                        Console.WriteLine("No rows found associate with the client name: " + fullName);
                        return clients;
                    }

                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        clients.Add(MapClient(reader));
                    }
                    Console.WriteLine("Client search complete");
                    message = "Client search successful";
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine($"Error in {nameof(SearchByFullName)}: {message}");
        }

        return clients;
    }

    private Client MapClient(MySqlDataReader reader)
    {
        Client client = new Client
        {
            MembershipId = Convert.ToInt32(reader["membership_id"]),

            FullName = reader["full_name"] != DBNull.Value ? reader["full_name"].ToString() : null,
            MembershipTypeId = reader["membership_type_id"] != DBNull.Value
                ? Convert.ToInt32(reader["membership_type_id"])
                : null,
            Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
            PhoneNumber = reader["phone_number"] != DBNull.Value ? reader["phone_number"].ToString() : null,
            Age = reader["age"] != DBNull.Value ? Convert.ToInt32(reader["age"]) : null,
            MembershipStart = reader["membership_start"] != DBNull.Value
                ? Convert.ToDateTime(reader["membership_start"])
                : null,
            MembershipEnd = reader["membership_end"] != DBNull.Value
                ? Convert.ToDateTime(reader["membership_end"])
                : null,
            MembershipStatus = reader["membership_status"] != DBNull.Value
                ? reader["membership_status"].ToString()
                : null,
            ProfilePictureByte = reader["profile_pic"] != DBNull.Value ? (byte[])reader["profile_pic"] : null
        };
        
        client.SetMembership(reader["membership"] != DBNull.Value ? reader["membership"].ToString() : null);
        client.SetProfilePictureImage(null);         // set as null default

        // Converge image byte from database into readable image
        if (client.ProfilePictureByte != null && client.ProfilePictureByte.Length > 0)
        {
            var image = imageViewer.ConvertByteArrayToImage(client.ProfilePictureByte);
            client.SetProfilePictureImage(image != null ? image : null);  // Set image or null
        } 
        
        return client;
    }
}