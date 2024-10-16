using System.ComponentModel;

namespace CarlosYulo.backend;

public class EmployeeAttendance
{
    public int employeeId { get; set; }
    public string fullName { get; set; }
    public string employeeType { get; set; }
    public string status { get; set; }
    public string attendanceStatus { get; set; }
    public DateTime date { get; set; }
    public DateTime checkInTime { get; set; }
    public DateTime checkOutTime { get; set; }

    public EmployeeAttendance()
    {
    }

    public override string ToString()
    {
        return $"Employee ID: {employeeId}, " +
               $"Full Name: {fullName}, " +
               $"Employee Type: {employeeType}, " +
               $"Status: {status}, " +
               $"Attendance Status: {attendanceStatus}, " +
               $"Date: {date.ToShortDateString()}, " +
               $"Check-In Time: {checkInTime.ToShortTimeString()}, " +
               $"Check-Out Time: {checkOutTime.ToShortTimeString()}";
    }
}

public enum AttendanceStatus
{
    [Description("Present")] PRESENT,
    [Description("Absent")] ABSENT,
    [Description("late")] LATE
}

public enum AttendanceType
{
    DAILY,
    MONTHLY,
    ALL_DAILY,
    ALL_MONTHLY
}