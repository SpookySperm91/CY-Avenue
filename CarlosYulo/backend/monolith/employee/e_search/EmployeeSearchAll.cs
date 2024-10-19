using System.Data;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.search;

public class EmployeeSearchAll 
{
    private readonly DatabaseConnection dbConnection;
    private readonly ImageViewer imageViewer;

    public EmployeeSearchAll(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
        imageViewer = new ImageViewer();
    }

    private int procudereTypes(EmployeTypeEnum? type)
    {
        return type switch
        {
            EmployeTypeEnum.STAFF => 1,
            EmployeTypeEnum.MANAGER => 2,
            EmployeTypeEnum.TRAINER => 3,
            EmployeTypeEnum.PERSONAL_TRAINER => 4,
            EmployeTypeEnum.ALL_OFFICE => 5,
            EmployeTypeEnum.ALL_TRAINERS => 6,
            EmployeTypeEnum.ALL => 7,
            null => 7,
            _ => 7
        };
    }


    public List<Employee> SearchAll(EmployeTypeEnum? type)
    {
        var employees = new List<Employee>();
        int procedureParameter = procudereTypes(type);

        try
        {
            // encase with 'using' to ensure proper dispose
            using (MySqlCommand command = new MySqlCommand("prcEmployeeSearchAll", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_type_id", procedureParameter); 

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Retrieve rows and set in object
                    while (reader.Read())
                    {
                        employees.Add(MapEmployee(reader));
                    }

                    Console.WriteLine("Retrieving and adding multiple rows to the list successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving employees: {ex.Message}");
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

public enum EmployeTypeEnum
{
    STAFF,
    MANAGER,
    TRAINER,
    PERSONAL_TRAINER,
    ALL_OFFICE,
    ALL_TRAINERS,
    ALL
}