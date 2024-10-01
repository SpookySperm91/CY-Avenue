using System.ComponentModel;

namespace CarlosYulo.backend;

public class EmployeeAttendance
{
    private int date { get; set; }
    private int employeeId { get; set; }
    private string fullName { get; set; }
    private AttendanceStatus attendanceStatus { get; set; }
    private DateTime checkInTime { get; set; }
    private DateTime checkOutTime { get; set; }

    public EmployeeAttendance()
    {
    }
}

public enum AttendanceStatus
{
    [Description("Present")] PRESENT,
    [Description("Absent")] ABSENT,
    [Description("late")] LATE
}