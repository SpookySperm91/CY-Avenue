using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientSearchAll 
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public ClientSearchAll(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    private string procudereTypes(string? type)
    {
        if (string.IsNullOrEmpty(type) || type.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return "prcClientSearchAll";
        }

        if (type.Equals("walk-in", StringComparison.OrdinalIgnoreCase))
        {
            return "prcClientSearchAllByWalkIn";
        }

        if (type.Equals("members", StringComparison.OrdinalIgnoreCase))
        {
            return "prcClientSearchAllByMembership";
        }

        // Default procedure
        return "prcClientSearchAll";
    }


    public List<Client> SearchAll(string type)
    {
        var clients = new List<Client>();
        string procedure = procudereTypes(type);

        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand(procedure, dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        clients.Add(MapClient(reader));
                    }

                    Console.WriteLine("Retrieving and add multiple rows in list successfully");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
        client.SetProfilePicture(null); // set as null default

        // Converge image byte from database into readable image
        if (client.ProfilePictureByte != null && client.ProfilePictureByte.Length > 0)
        {
            var image = imageViewer.ConvertByteArrayToImage(client.ProfilePictureByte);
            client.SetProfilePictureImage(image != null ? image : null); // Set image or null
        }

        return client;
    }
}