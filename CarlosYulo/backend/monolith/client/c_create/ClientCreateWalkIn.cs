using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.create;

public class ClientCreateWalkIn
{
    private DatabaseConnection dbConnection;

    public ClientCreateWalkIn(DatabaseConnection databaseConnection)
    {
        this.dbConnection = databaseConnection;
    }


    public bool CreateNewWalkIn(Client? client, out string message)
    {
        // if client is null
        if (client is null)
        {
            message = "Client object is null";
            return false;
        }

        List<string> missingFields = new List<string>();
        ValidateFields(client, missingFields);

        if (missingFields.Count > 0)
        {
            message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
            return false;
        }

        // START OPERATION
        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcClientCreateNew", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // add output parameters
                MySqlParameter outputId = new MySqlParameter("p_membership_id", MySqlDbType.Int32)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputId);

                MySqlParameter outputMembership = new MySqlParameter("p_membership", MySqlDbType.VarChar, 55)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputMembership);

                // instantiate values to prc parameter
                NewClientDataMap(command, client);

                // execute
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    client.MembershipId = Convert.ToInt32(outputId.Value);
                    client.SetMembership(outputMembership.Value?.ToString());

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"New walk-in client {client.FullName} created successfully";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"New walk-in client {client.FullName} was not created successfully";
                return false;
            }
        }
        catch (Exception e)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction?.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = e.Message;
            return false;
        }
    }

    private void NewClientDataMap(MySqlCommand command, Client client)
    {
        DateTime expireTime = DateTime.Now.AddHours(12);
        string membershipStatus = "Active";

        command.Parameters.AddWithValue("p_full_name", client.FullName?.TrimEnd());
        command.Parameters.AddWithValue("p_membership_type_id", client.MembershipTypeId);
        command.Parameters.AddWithValue("p_email", client.Email?.TrimEnd());
        command.Parameters.AddWithValue("p_phone_number", client.PhoneNumber?.TrimEnd());
        command.Parameters.AddWithValue("p_gender", client.Gender?.TrimEnd());
        command.Parameters.AddWithValue("p_age", client.Age ?? (object)DBNull.Value);

        // non important
        command.Parameters.AddWithValue("p_birthday", null);
        command.Parameters.AddWithValue("p_profile_pic", null);

        command.Parameters.AddWithValue("p_membership_start", DateTime.Now);
        command.Parameters.AddWithValue("p_membership_end", expireTime);
        command.Parameters.AddWithValue("p_membership_status", membershipStatus);
    }

    private void ValidateFields(Client client, List<string> missingFields)
    {
        if (client.MembershipTypeId is null  || client.MembershipTypeId == 1 || client.MembershipTypeId == 2)
        {
            missingFields.Add("Invalid membership type");
        }
        
        if (string.IsNullOrWhiteSpace(client.FullName))
        {
            missingFields.Add("Full Name");
        }

        if (string.IsNullOrWhiteSpace(client.PhoneNumber) && string.IsNullOrWhiteSpace(client.Email))
        {
            missingFields.Add("Either Phone Number or Email must be provided");
        }

        if (client.Age == null || client.Age < 10)
        {
            missingFields.Add("Age");
        }

        if (string.IsNullOrWhiteSpace(client.Gender))
        {
            missingFields.Add("Gender");
        }
    }
}