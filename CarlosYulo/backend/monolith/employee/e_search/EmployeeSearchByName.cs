using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.search;

public class EmployeeSearchByName
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public EmployeeSearchByName(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    public List<Employee> SearchByFullName(string fullName, int? employeeType, out string message)
    {
        var employees = new List<Employee>();

        // START OPERATION
        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand("prcEmployeeSearchByName", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_full_name", fullName.TrimEnd());
                command.Parameters.AddWithValue("p_employee_type_id", employeeType);

                // MySqlDataReader read response
                // store rows in list
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        message = "Employee search fail. " + fullName + " not found";
                        Console.WriteLine("No rows found associate with the employee name: " + fullName);
                        return employees;
                    }

                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        employees.Add(MapEmployee(reader));
                    }
                    Console.WriteLine("Employee search successful");
                    message = "Employee search successful";
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine($"Error in {nameof(SearchByFullName)}: {message}");
        }
        return employees;
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