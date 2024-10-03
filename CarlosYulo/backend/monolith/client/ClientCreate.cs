using System.Diagnostics;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith
{
    public class ClientCreate : ICreate<Client>, IClientCreate
    {
        private DatabaseConnector dbConnector;

        public ClientCreate(DatabaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }


        public bool CreateWalkIn(Client client)
        {
            bool result = CreateClient(client, "Walk-in");
            if (!result)
                MessageBox.Show("Error creating walk-in account. Missing information", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            return result;
        }


        public bool Create(Client client)
        {
            bool result = CreateClient(client, "Membership");
            if (!result)
                MessageBox.Show("Error creating membership account. Missing information", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            return result;
        }


        private bool CreateClient(Client client, string type)
        {
            try
            {
                using (var connection = dbConnector.CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand("prcClientCreateNew", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        DateTime expireTime = new DateTime();

                        if (type.Equals("Walk-in"))
                        {
                            expireTime = DateTime.Today;
                        }
                        else if (type.Equals("Membership"))
                        {
                            expireTime = DateTime.Now.AddDays(30);
                        }

                        MembershipStatus status = MembershipStatus.ACTIVE;
                        string statusString = status.GetDescription();

                        // Add the output parameter for membership ID
                        MySqlParameter outputIdParam = new MySqlParameter("p_membership_id", MySqlDbType.Int32);
                        outputIdParam.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(outputIdParam);

                        command.Parameters.AddWithValue("p_full_name", client.FullName);
                        command.Parameters.AddWithValue("p_membership_type_id", client.MembershipType);
                        command.Parameters.AddWithValue("p_email", client.Email);
                        command.Parameters.AddWithValue("p_phone_number", client.PhoneNumber);
                        command.Parameters.AddWithValue("p_gender", client.Gender);

                        command.Parameters.AddWithValue("p_membership_start", DateTime.Now);
                        command.Parameters.AddWithValue("p_membership_end", expireTime);
                        command.Parameters.AddWithValue("p_membership_status", statusString);

                        if (type.Equals("Membership"))
                        {
                            command.Parameters.AddWithValue("p_age", client.Age);
                            command.Parameters.AddWithValue("p_birthday", client.BirthDate);
                            command.Parameters.AddWithValue("p_profile_pic", client.ProfilePicture);
                        }
                        else if (type.Equals("Walk-in"))
                        {
                            command.Parameters.AddWithValue("p_age", null);
                            command.Parameters.AddWithValue("p_birthday", null);
                            command.Parameters.AddWithValue("p_profile_pic", null);
                        }

                        // Execute the stored procedure
                        int rowsAffected = command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        if (outputIdParam.Value != DBNull.Value)
                        {
                            client.MembershipId = Convert.ToInt32(outputIdParam.Value);
                        }

                        // Return true if insert was successful, otherwise false
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error creating client: {ex.Message}");
                return false;
            }
        }
    }
}