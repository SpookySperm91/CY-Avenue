using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.search;

public class EmployeeSearchById 
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public EmployeeSearchById(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }


    public Employee? SearchById(int employeeId, int? employeeType, out string message)
    {
        if (employeeId.ToString().Length != 6)
        {
            message = "Invalid employee ID input. Please try again. Members ID must be 6 digits.";
            return null;
        }

        Employee? employee = null;
        try
        {
            using (MySqlCommand command = new MySqlCommand("prcEmployeeSearchByEmployeeId", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_id", employeeId);
                command.Parameters.AddWithValue("p_employee_type_id", employeeType);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)
                    {
                        message = "Employee search failed. Employee-ID: " + employeeId + " is not found";
                        Console.WriteLine("No row found associated with the employee id: " + employeeId);
                        return employee;
                    }

                    if (reader.Read())
                    {
                        employee = MapEmployee(reader);
                        message = "Employee search successful";
                        return employee;
                    }

                    message = "Error shit";
                    return employee;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private Employee MapEmployee(MySqlDataReader reader)
    {
        Employee employee = new Employee
        {
            EmployeeId = Convert.ToInt32(reader["employee_id"]),
            FullName = reader["full_name"] != DBNull.Value ? reader["full_name"].ToString() : null,
            EmployeeTypeId = reader["employee_type_id"] != DBNull.Value
                ? Convert.ToInt32(reader["employee_type_id"])
                : null,
            Salary = reader["salary"] != DBNull.Value ? Convert.ToDouble(reader["salary"]) : null,
            Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
            PhoneNumber = reader["phone_number"] != DBNull.Value ? reader["phone_number"].ToString() : null,
            Age = reader["age"] != DBNull.Value ? Convert.ToInt32(reader["age"]) : null,
            BirthDate = reader["birthday"] != DBNull.Value ? Convert.ToDateTime(reader["birthday"]) : null,
            ProfilePictureByte = reader["profile_pic"] != DBNull.Value ? (byte[])reader["profile_pic"] : null
        };

        employee.SetEmployeeType(reader["employee_type"] != DBNull.Value ? reader["employee_type"].ToString() : null);
        employee.SetProfilePictureImage(null); // Set null by default

        // Convert image byte into image
        if (employee.ProfilePictureByte != null && employee.ProfilePictureByte.Length > 0)
        {
            var image = imageViewer.ConvertByteArrayToImage(employee.ProfilePictureByte);
            employee.SetProfilePictureImage(image);
        }

        return employee;
    }
}