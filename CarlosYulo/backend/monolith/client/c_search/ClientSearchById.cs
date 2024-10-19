using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientSearchById 
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public ClientSearchById(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    public Client? SearchById(int membershipId, string? gender, out string message)
    {
        if (membershipId.ToString().Length != 6)
        {
            message = "Invalid membership ID input. Please try again. Members ID must be 6 digits.";
            return null;
        }

        Client? client = null;
        try
        {
            using (MySqlCommand command =
                   new MySqlCommand("prcClientSearchByMembershipId", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_membership_id", membershipId);
                command.Parameters.AddWithValue("p_gender", gender);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)    
                    {
                        message = "Client search failed. Membership-ID: " + membershipId + " is not found";
                        Console.WriteLine("No row found associated with the client id: " + membershipId);
                        return client;
                    }

                    if (reader.Read())
                    {
                        client = MapClient(reader);
                        message = "Client search successful";
                        return client;
                    }

                    message = "Error shit";
                    return client;
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine(message);
            return client;
        }
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
        client.SetProfilePictureImage(null); // set as null default

        // Converge image byte from database into readable image
        if (client.ProfilePictureByte != null && client.ProfilePictureByte.Length > 0)
        {
            var image = imageViewer.ConvertByteArrayToImage(client.ProfilePictureByte);
            client.SetProfilePictureImage(image != null ? image : null); // Set image or null
        }

        return client;
    }
}