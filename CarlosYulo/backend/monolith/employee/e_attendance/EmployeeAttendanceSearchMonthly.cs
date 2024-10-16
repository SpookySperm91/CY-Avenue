using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.attendance;

public class EmployeeAttendanceSearchMonthly
{
    private DatabaseConnection dbConnection;

    public EmployeeAttendanceSearchMonthly(DatabaseConnection dbConnection)
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

    public List<EmployeeAttendance>? SearchEmployeeMonthlyAttendance(int employeeId, DateTime attendanceDate,
        out string message)
    {
        var employeeAttendances = new List<EmployeeAttendance>();
        message = string.Empty;

        if (!CheckCondition(employeeId, attendanceDate, out message))
        {
            return null;
        }

        // START OPERATION
        try
        {
            // encapsulate in 'using' to ensure resources are released properly
            using (var command = new MySqlCommand("prcEmployeeCheckMonthlyAttendance", dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_id", employeeId);
                command.Parameters.AddWithValue("p_month_date", attendanceDate);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // If no rows are returned
                    if (!reader.HasRows)
                    {
                        message =
                            $"No attendance records found for Employee ID {employeeId} for the month of {attendanceDate.ToString("MMMM yyyy")}.";
                        return null;
                    }

                    // Retrieve rows and map to objects
                    while (reader.Read())
                    {
                        employeeAttendances.Add(MapAttendance(reader));
                    }

                    message = "Attendance records retrieved successfully.";
                    return employeeAttendances;
                }
            }
        }
        catch (MySqlException sqlEx)
        {
            message = $"Database error occurred: {sqlEx.Message}";
            Console.WriteLine("MySqlException: " + sqlEx);
            return null;
        }
        catch (Exception ex)
        {
            message = $"An unexpected error occurred: {ex.Message}";
            Console.WriteLine("Exception: " + ex);
            return null;
        }
    }

    private EmployeeAttendance MapAttendance(MySqlDataReader reader)
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