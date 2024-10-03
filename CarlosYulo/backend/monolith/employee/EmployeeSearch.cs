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

    public Employee? SearchById(int id, int? employeeTypeId) 
    {
        return SearchEmployee("prcEmployeeSearchByEmployeeId", id, null, employeeTypeId);
    }

    public Employee? SearchByFullName(string fullName, int? employeeTypeId) 
    {
        return SearchEmployee("prcEmployeeSearchByName", null, fullName, employeeTypeId);
    }

    public List<Employee> SearchAll()
    {
        return new List<Employee>();
    }

    private Employee? SearchEmployee(string storedProcedure, int? employeeId, string? fullName, int? employeeTypeId)
    {
        Employee? employee = null;

        using (var connection = dbConnector.CreateConnection())
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

                // Gender parameter (common to both procedures)
                command.Parameters.AddWithValue("p_employee_type_id",
                    employeeTypeId.HasValue? employeeTypeId.Value : employeeTypeId);

                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee();
                            employee.EmployeeFullName = reader["full_name"].ToString();
                            employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                            employee.Email = reader["email"].ToString();
                            employee.PhoneNumber = reader["phone_number"].ToString();
                            employee.Age = Convert.ToInt32(reader["age"]);
                            employee.Gender = reader["gender"].ToString();
                            // employee.MembershipStart = Convert.ToDateTime(reader["membership_start"]);
                            // employee.MembershipEnd = Convert.ToDateTime(reader["membership_end"]);
                            // client.MembershipStatus = reader["membership_status"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching client: {ex.Message}");
                }
            }
        }

        return employee;
    }
}