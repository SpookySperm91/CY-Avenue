using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.attendance;

public class EmployeeAttendanceSearchAll
{
    private DatabaseConnection dbConnection;

    public EmployeeAttendanceSearchAll(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    private string procedureType(AttendanceType? procedure)
    {
        return procedure switch
        {
            AttendanceType.ALL_DAILY => "prcEmployeeCheckAllDailyAttendance",
            AttendanceType.ALL_MONTHLY => "prcEmployeeCheckAllMonthlyAttendance",
            _ => "prcEmployeeCheckAllMonthlyAttendance",
        };
    }


    public List<EmployeeAttendance>? SearchAll(DateTime checkDate, AttendanceType? procedure, out string message)
    {
        var employee = new List<EmployeeAttendance>();
        string prc = procedureType(procedure);

        try
        {
            // encase with 'using' to ensure proper dispose
            using (var command = new MySqlCommand(prc, dbConnection.mysqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_date", checkDate);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // retrieve rows and set in object
                    while (reader.Read())
                    {
                        employee.Add(MapAttendance(reader));
                    }

                    Console.WriteLine("Retrieving and add multiple rows in list successfully");
                }
            }

            message = "";
            return employee;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            message = ex.Message;
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