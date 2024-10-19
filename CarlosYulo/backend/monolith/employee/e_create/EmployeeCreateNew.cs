using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.create;

public class EmployeeCreateNew
{
    private DatabaseConnection dbConnection;
    private ImageViewer imageViewer;

    public EmployeeCreateNew(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    public bool Create(Employee employee, out string message)
    {
        // if employee is null
        if (employee is null)
        {
            message = "Client object is null";
            return false;
        }

        List<string> missingFields = new List<string>();
        ValidateFields(employee, missingFields);

        // Create error message
        if (missingFields.Count > 0)
        {
            message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
            return false;
        }
        string employeeType = employee.EmployeeTypeId switch
        {
            1 => "Manager",
            2 => "Staff",
            3 => "Trainer",
            4 => "Personal Trainer",
            _ => "Unknown" 
        };


        // START OPERATION
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcEmployeeCreateNew",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                // add output parameters
                MySqlParameter outputEmployeeId = new MySqlParameter("p_employee_id", MySqlDbType.Int32)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputEmployeeId);

                MySqlParameter outputEmployeeType = new MySqlParameter("p_employee_type", MySqlDbType.VarChar)
                    { Direction = ParameterDirection.Output };
                command.Parameters.Add(outputEmployeeType);

                // instantiate values to prc parameter
                NewEmployeeDataMap(command, employee);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    employee.EmployeeId = Convert.ToInt32(outputEmployeeId.Value);
                    employee.SetEmployeeType(outputEmployeeType.Value?.ToString());

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"New {employeeType}: {employee.FullName} created successfully";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Failed to create new {employeeType}";
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


    private void NewEmployeeDataMap(MySqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("p_full_name", employee.FullName?.TrimEnd());
        command.Parameters.AddWithValue("p_employee_type_id", employee.EmployeeTypeId);
        command.Parameters.AddWithValue("p_salary", employee.Salary);
        command.Parameters.AddWithValue("p_email", employee.Email?.TrimEnd());
        command.Parameters.AddWithValue("p_phone_number", employee.PhoneNumber?.TrimEnd());
        command.Parameters.AddWithValue("p_gender", employee.Gender?.TrimEnd());
        command.Parameters.AddWithValue("p_age", employee.Age ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_birthday", employee.BirthDate?.Date ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_profile_pic", employee.ProfilePictureByte);
    }


    private void ValidateFields(Employee employee, List<string> missingFields)
    {
        if (!employee.EmployeeTypeId.HasValue || employee.EmployeeTypeId > 4 )
        {
            missingFields.Add("Invalid Employee Type");
        }

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

        if (employee.Age == null || employee.Age < 10)
        {
            missingFields.Add("Age");
        }

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