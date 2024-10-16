using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.attendance;

public class EmployeeAttendanceCreate
{
    private DatabaseConnection dbConnection;

    public EmployeeAttendanceCreate(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    public bool CreateEmployeeAttendance(int? employeeId, AttendanceStatus attendanceStatus,
        out string message)
    {
        if (!employeeId.HasValue)
        {
            message = "Employee ID are missing";
            return false;
        }

        return CreateAttendance(employeeId, attendanceStatus, out message);
    }


    private bool CreateAttendance(int? employeeId, AttendanceStatus attendanceStatus,
        out string message)
    {
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcEmployeeCreateAttendance",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                EmployeeAttendanceMap(command, employeeId, attendanceStatus);

                // execute query and set transaction to null after
                int row = command.ExecuteNonQuery();

                if (row > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    Console.WriteLine("Employee ID: " + employeeId + " attendance successfully created.");
                    message = $"Employee ID: {employeeId} attendance successfully created.";
                    return true;
                }
            }

            dbConnection.transaction.Rollback();
            dbConnection.transaction = null;
            message = "Employee ID: " + employeeId + " attendance could not be created";
            return false;
        }
        catch (Exception e)
        {
            // Roll back if error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            // Handle general errors
            Console.WriteLine("Error: " + e.Message);
            message = "Error: " + e.Message;
            return false;
        }
    }

    private void EmployeeAttendanceMap(MySqlCommand command, int? employeeId, AttendanceStatus attendanceStatus)
    {
        string status = attendanceStatus switch
        {
            AttendanceStatus.PRESENT => "PRESENT",
            AttendanceStatus.LATE => "LATE",
            AttendanceStatus.ABSENT => "ABSENT",
            _ => throw new ArgumentException("Invalid attendance status")
        };

        command.Parameters.AddWithValue("p_employee_id", employeeId);
        command.Parameters.AddWithValue("p_check_in_time", DateTime.Now.TimeOfDay);
        command.Parameters.AddWithValue("p_check_out_time", DBNull.Value);
        command.Parameters.AddWithValue("p_attendance_status", status);
    }
}