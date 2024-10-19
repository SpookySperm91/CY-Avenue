using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.attendance;

public class EmployeeAttendanceSearchDaily
{
    private DatabaseConnection dbConnection;

    public EmployeeAttendanceSearchDaily(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    private bool CheckCondition(int employeeId, DateTime attendanceDate, out string message)
    {
        message = string.Empty;

        if (employeeId.ToString().Length != 6)
        {
            message = "Invalid employee ID. It must be 6 digit";
            return false;
        }

        if (attendanceDate == DateTime.MinValue || attendanceDate.Date > DateTime.Now.Date)
        {
            message = "Attendance date cannot be null or in the future.";
            return false;
        }
        
        return true;
    }


    public EmployeeAttendance? SearchEmployeeDailyAttendance(int employeeId, DateTime attendanceDate,
        out string message)
    {
        if (!CheckCondition(employeeId, attendanceDate, out message))
        {
            return null;
        }

        EmployeeAttendance? employee = null;
        try
        {
            using (MySqlCommand command =
                   new MySqlCommand("prcEmployeeCheckDailyAttendance", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_id", employeeId);
                command.Parameters.AddWithValue("p_date", attendanceDate);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // return error if no rows
                    if (!reader.HasRows)
                    {
                        message = "Employee attendance for employee-id: " + employeeId + " is not found";
                        Console.WriteLine("No row found associated with the employee id: " + employeeId);
                        return employee;
                    }

                    if (reader.Read())
                    {
                        employee = MapEmployeeAttendance(reader);
                        message = "Client search successful";
                        return employee;
                    }

                    message = "Error shit";
                    return null;
                }
            }
        }
        catch (Exception e)
        {
            message = "An error occurred: " + e.Message;
            Console.WriteLine(message);
            return null;
        }
    }

    private EmployeeAttendance MapEmployeeAttendance(MySqlDataReader reader)
    {
        return new EmployeeAttendance()
        {
            employeeId = reader.GetInt32("employee_id"),
            fullName = reader.GetString("full_name"),
            employeeType = reader.GetString("employee_type"),
            attendanceStatus = reader.GetString("attendance_status"),
            date = reader.GetDateTime("date"),

            checkInTime = reader.IsDBNull(reader.GetOrdinal("check_in_time"))
                ? default
                : DateTime.Today.Add(reader.GetTimeSpan("check_in_time")),

            checkOutTime = reader.IsDBNull(reader.GetOrdinal("check_out_time"))
                ? default
                : DateTime.Today.Add(reader.GetTimeSpan("check_out_time")),
        };
    }
}