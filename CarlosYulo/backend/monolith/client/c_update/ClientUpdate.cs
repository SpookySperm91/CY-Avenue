using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientUpdate : IUpdate<Client>
{
    private DatabaseConnector dbConnector;
    private DatabaseConnection dbConnection;

    public ClientUpdate(DatabaseConnector dbConnector, DatabaseConnection dbConnection)
    {
        this.dbConnector = dbConnector;
        this.dbConnection = dbConnection;
    }

    public bool UpdateProfilePicture(Client client, string image, out string message)
    {
        if (client.SetProfilePicture(image, out message))
        {
            return UpdateClient("prcClientUpdateProfilePicture", client, null, out message); // Save in database directly
        }
        return false;
    }


    public bool UpdateClientMembershipType(Client client, MembershipType membership, out string message)
    {
        // Validate client and membership objects
        if (client == null || membership == null)
        {
            Console.WriteLine("Client or membership cannot be null.");
            message = "Client or membership cannot be null.";
            return false;
        }

        // Assuming you have a stored procedure that handles the membership update
        return UpdateClient("prcClientUpdateMembership", client, membership, out message);
    }


    public bool Update(Client client)
    {
        if (client == null)
        {
            Console.WriteLine("Client cannot be null.");
            return false;
        }

        string message;
        return UpdateClient("prcClientUpdateData", client, null, out message);
    }


    private bool UpdateClient(string storedProcedure, Client client, MembershipType? membershipType, out string message)
    {
        using (var connection = dbConnector.CreateConnection())
        {
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    switch (storedProcedure)
                    {
                        case "prcClientUpdateMembership":
                            return UpdateMembership(command, client, membershipType, out message);

                        case "prcClientUpdateProfilePicture":
                            return UpdateProfilePicture(command, client, out message);

                        case "prcClientUpdateData":
                            return UpdateClientData(command, client, out message);

                        default:
                            message = "Unknown stored procedure.";
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                message = $"Error fetching membership: {ex.Message}";
                return false;
            }
        }
    }

    private bool UpdateMembership(MySqlCommand command, Client client, MembershipType? membershipType,
        out string message)
    {
        if (membershipType == null)
        {
            message = "Membership type must be specified.";
            return false;
        }

        int membershipTypeId = membershipType.Value switch
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
        command.Parameters.AddWithValue("p_membership_end", DateTime.Now.AddYears(30));
        command.Parameters.AddWithValue("p_membership_status", statusString);

        MySqlParameter outputMembershipParam = new MySqlParameter("p_membership", MySqlDbType.VarChar, 55)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputMembershipParam);

        command.ExecuteNonQuery();

        client.SetMembership(outputMembershipParam.Value.ToString());
        message = $"Membership updated successfully to {membershipType}.";
        return true;
    }

    private bool UpdateProfilePicture(MySqlCommand command, Client client, out string message)
    {
        if (client.ProfilePictureByte == null || client.ProfilePictureByte.Length == 0)
        {
            message = "No profile picture data to update.";
            return false;
        }

        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_profile_pic", client.ProfilePictureByte);

        command.ExecuteNonQuery();
        message = "Profile picture updated successfully.";
        return true;
    }

    private bool UpdateClientData(MySqlCommand command, Client client, out string message)
    {
        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_full_name", client.FullName?.TrimEnd());
        command.Parameters.AddWithValue("p_membership_type_id", client.MembershipTypeId);
        command.Parameters.AddWithValue("p_email", client.Email?.TrimEnd());
        command.Parameters.AddWithValue("p_phone_number", client.PhoneNumber?.TrimEnd());
        command.Parameters.AddWithValue("p_gender", client.Gender?.TrimEnd());
        command.Parameters.AddWithValue("p_age", client.Age ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_birthday", client.BirthDate?.Date ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_profile_pic", client.ProfilePictureByte);

        command.ExecuteNonQuery();
        message = "Client data updated successfully.";
        return true;
    }
}