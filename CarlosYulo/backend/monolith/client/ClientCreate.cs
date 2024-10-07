using System.Diagnostics;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith
{
    public class ClientCreate : ICreate<Client>, IClientCreate
    {
        private DatabaseConnector dbConnector;
        private ImageViewer imageViewer;

        public ClientCreate(DatabaseConnector dbConnector)
        {
            this.dbConnector = dbConnector;
            imageViewer = new ImageViewer();
        }

        // create new walk-in client
        public bool CreateWalkIn(Client client, out string message)
        {
            return CreateClient(client, "Walk-in", out message);
        }

        // create new member client
        public bool Create(Client client, out string message)
        {
            return CreateClient(client, "Membership", out message);
        }

        
        // base class for creating
        private bool CreateClient(Client client, string type, out string message)
        {
            List<string> missingFields = new List<string>();

            // Check for missing fields
            ValidateBasicFields(client, missingFields);
            ValidateAge(client, missingFields);
            if (type.Equals("Membership", StringComparison.OrdinalIgnoreCase))
            {
                ValidateMembershipFields(client, missingFields);
            }

            // Create error message
            if (missingFields.Count > 0)
            {
                message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
                return false;
            }

            // Start operation
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
                            expireTime = DateTime.Today.AddHours(12);
                        }
                        else if (type.Equals("Membership"))
                        {
                            expireTime = DateTime.Now.AddDays(30);
                        }

                        // Turn enum into string
                        MembershipStatus status = MembershipStatus.ACTIVE;
                        string statusString = status.ToString().Substring(0, 1).ToUpper() +
                                              status.ToString().Substring(1).ToLower();

                        // Add the output parameter for membership ID
                        MySqlParameter outputIdParam = new MySqlParameter("p_membership_id", MySqlDbType.Int32);
                        outputIdParam.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(outputIdParam);

                        MySqlParameter outputMembershipParam = new MySqlParameter("p_membership", MySqlDbType.VarChar, 55);
                        outputMembershipParam.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(outputMembershipParam);


                        command.Parameters.AddWithValue("p_full_name", client.FullName.TrimEnd());
                        command.Parameters.AddWithValue("p_membership_type_id", client.MembershipTypeId);
                        command.Parameters.AddWithValue("p_email", client.Email.TrimEnd());
                        command.Parameters.AddWithValue("p_phone_number", client.PhoneNumber.TrimEnd());
                        command.Parameters.AddWithValue("p_gender", client.Gender.TrimEnd());

                        command.Parameters.AddWithValue("p_membership_start", DateTime.Now);
                        command.Parameters.AddWithValue("p_membership_end", expireTime);
                        command.Parameters.AddWithValue("p_membership_status", statusString);

                        if (type.Equals("Membership"))
                        {
                            command.Parameters.AddWithValue("p_age", client.Age);
                            command.Parameters.AddWithValue("p_birthday", client.BirthDate);
                            command.Parameters.AddWithValue("p_profile_pic", client.ProfilePictureByte);
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
                        if (outputIdParam.Value != DBNull.Value && outputMembershipParam.Value != DBNull.Value)
                        {
                            client.MembershipId = Convert.ToInt32(outputIdParam.Value);
                            client.SetMembership(outputMembershipParam.Value != DBNull.Value
                                ? outputMembershipParam.Value.ToString()
                                : null);
                        }

                        // Return true if insert was successful, otherwise false
                        if (rowsAffected > 0)
                        {
                            message = "Client creation success";
                            return true;
                        }

                        message = "Client creation failed";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        
        
        
        
        
        
        
        
        
        
        

        // Method to validate basic fields
        private void ValidateBasicFields(Client client, List<string> missingFields)
        {
            if (string.IsNullOrWhiteSpace(client.FullName))
            {
                missingFields.Add("Full Name");
            }

            if (string.IsNullOrWhiteSpace(client.Email))
            {
                missingFields.Add("Email");
            }

            if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            {
                missingFields.Add("Phone Number");
            }
        }

        // Method to validate age
        private void ValidateAge(Client client, List<string> missingFields)
        {
            if (client.Age == null || client.Age < 10)
            {
                missingFields.Add("Age");
            }
        }

        // Method to validate membership-specific fields
        private void ValidateMembershipFields(Client client, List<string> missingFields)
        {
            if (string.IsNullOrWhiteSpace(client.Gender))
            {
                missingFields.Add("Gender");
            }

            if (client.BirthDate == null)
            {
                missingFields.Add("Birth Date");
            }

            if (client.ProfilePictureByte != null && client.ProfilePictureByte.Length > 0 &&
                !imageViewer.IsValidImageFormat(client.ProfilePictureByte))
            {
                missingFields.Add("Profile Picture (must be a valid PNG or JPEG)");
            }
        }
    }
}