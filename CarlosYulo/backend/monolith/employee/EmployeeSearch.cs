using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeSearch : ISearch<Employee, int?>
{
    private DatabaseConnector dbConnector;

    public EmployeeSearch(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }

    public Employee? SearchById(int id, int? employeeTypeId, out string message)
    {
        return SearchEmployee("prcEmployeeSearchByEmployeeId", id, null, employeeTypeId, out message);
    }

    public Employee? SearchByFullName(string fullName, int? employeeTypeId, out string message)
    {
        return SearchEmployee("prcEmployeeSearchByName", null, fullName, employeeTypeId, out message);
    }

    public List<Employee> SearchAll(string wala)
    {
        return new List<Employee>();
    }

    private Employee? SearchEmployee(string storedProcedure, int? employeeId, string? fullName, int? employeeTypeId,
        out string message)
    {
        Employee? employee = null;
        message = string.Empty;

        using (var connection = dbConnector.CreateConnection())
        {
            try
            {
                // Open the connection
                connection.Open();

                using (var command = new MySqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters based on search type
                    if (employeeId.HasValue)
                    {
                        command.Parameters.AddWithValue("p_employee_id", employeeId.Value);
                    }
                    else if (!string.IsNullOrEmpty(fullName))
                    {
                        command.Parameters.AddWithValue("p_employee_full_name", fullName);
                    }

                    // Add employeeTypeId parameter
                    command.Parameters.AddWithValue("p_employee_type_id",
                        employeeTypeId.HasValue ? employeeTypeId.Value : employeeTypeId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                EmployeeFullName = reader["full_name"] is DBNull ? null : reader["full_name"].ToString(),
                                EmployeeId = reader["employee_id"] is DBNull ? null : Convert.ToInt32(reader["employee_id"]),
                                Email = reader["email"] is DBNull ? null : reader["email"].ToString(),
                                PhoneNumber = reader["phone_number"] is DBNull ? null : reader["phone_number"].ToString(),
                                Age = reader["age"] is DBNull ? null : Convert.ToInt32(reader["age"]),
                                Gender = reader["gender"] is DBNull ? null : reader["gender"].ToString(),
                                ProfilePicture = reader["profile_pic"] is DBNull ? null : (byte[])reader["profile_pic"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = $"Error fetching employee: {ex.Message}";
            }
        }

        if (employee == null)
        {
            // If client is null, show an appropriate message based on what's available
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                message = "Employee username: " + fullName + " cannot be found";
            }
            else if (employeeId.HasValue)
            {
                message = "Employee ID: " + employeeId.Value + " cannot be found";
            }
            else
            {
                message = "Employee not found";
            }

            return employee;
        }

        message = "Employee found";
        return employee;
    }
}