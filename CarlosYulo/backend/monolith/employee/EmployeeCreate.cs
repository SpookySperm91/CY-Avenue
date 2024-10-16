using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeCreate : ICreate<Employee>
{
    private DatabaseConnector dbConnector;
    private ImageViewer imageViewer;

    public EmployeeCreate(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
        imageViewer = new ImageViewer();
    }

    public bool Create(Employee systemAccount, out string message)
    {
        // Check for missing fields
        List<string> missingFields = new List<string>();
        ValidateBasicFields(systemAccount, missingFields);
        ValidateAge(systemAccount, missingFields);

        // Create error message
        if (missingFields.Count > 0)
        {
            message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
            return false;
        }

        try
        {
            using (var connection = dbConnector.CreateConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand("prcEmployeeCreateNew", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    // Add the output parameter for employee ID
                    MySqlParameter outputIdParam = new MySqlParameter("p_employee_id", MySqlDbType.Int32);
                    outputIdParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(outputIdParam);

                    // Add the output parameter for employee type
                    MySqlParameter outputEmployeeTypeParam =
                        new MySqlParameter("p_employee_type", MySqlDbType.VarChar, 55);
                    outputEmployeeTypeParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(outputEmployeeTypeParam);

                    command.Parameters.AddWithValue("p_full_name", systemAccount.FullName.TrimEnd());
                    command.Parameters.AddWithValue("p_employee_type_id", systemAccount.EmployeeTypeId);
                    command.Parameters.AddWithValue("p_salary",
                        systemAccount.Salary.HasValue ? Math.Round(systemAccount.Salary.Value, 2) : DBNull.Value);
                    command.Parameters.AddWithValue("p_email", systemAccount.Email.TrimEnd());
                    command.Parameters.AddWithValue("p_phone_number", systemAccount.PhoneNumber.TrimEnd());
                    command.Parameters.AddWithValue("p_gender", systemAccount.Gender.TrimEnd());
                    command.Parameters.AddWithValue("p_age", systemAccount.Age);
                    command.Parameters.AddWithValue("p_birthday", systemAccount.BirthDate);
                    command.Parameters.AddWithValue("p_profile_pic", systemAccount.ProfilePictureByte);


                    int rowsAffected = command.ExecuteNonQuery();

                    // Retrieve the output parameter value
                    if (outputIdParam.Value != DBNull.Value && outputEmployeeTypeParam.Value != DBNull.Value)
                    {
                        systemAccount.EmployeeId = Convert.ToInt32(outputIdParam.Value);
                        systemAccount.SetEmployeeType(outputEmployeeTypeParam.Value != DBNull.Value
                            ? outputEmployeeTypeParam.Value.ToString()
                            : null);
                    }

                    // Check if rows were affected (indicates success)
                    if (rowsAffected > 0)
                    {
                        message = "Employee creation successful.";
                        return true;
                    }

                    message = "Employee creation failed.";
                    return false;
                }
            }
        }
        catch (MySqlException ex)
        {
            // Capture the error message raised by the stored procedure (e.g., duplicate entry)
            message = $"Database error: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            // Capture any general error
            message = $"Error: {ex.Message}";
            return false;
        }
    }


    // Method to validate basic fields
    private void ValidateBasicFields(Employee employee, List<string> missingFields)
    {
        if (string.IsNullOrWhiteSpace(employee.FullName))
        {
            missingFields.Add("Full Name");
        }

        if (string.IsNullOrWhiteSpace(employee.Email))
        {
            missingFields.Add("Email");
        }

        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
        {
            missingFields.Add("Phone Number");
        }
    }

    // Method to validate age
    private void ValidateAge(Employee client, List<string> missingFields)
    {
        if (client.Age == null || client.Age < 10)
        {
            missingFields.Add("Age");
        }
    }

    // Method to validate membership-specific fields
    private void ValidateMembershipFields(Employee employee, List<string> missingFields)
    {
        if (string.IsNullOrWhiteSpace(employee.Gender))
        {
            missingFields.Add("Gender");
        }

        if (employee.BirthDate == null)
        {
            missingFields.Add("Birth Date");
        }

        if (employee.ProfilePictureByte != null && employee.ProfilePictureByte.Length > 0 &&
            !imageViewer.IsValidImageFormat(employee.ProfilePictureByte))
        {
            missingFields.Add("Profile Picture (must be a valid PNG or JPEG)");
        }
    }
}