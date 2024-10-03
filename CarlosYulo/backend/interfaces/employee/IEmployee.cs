namespace CarlosYulo.backend.monolith.employee;

public interface IEmployee
{
}


public interface IEmployeeAttendance
{
    EmployeeAttendance? CheckEmployeeAttendanceDaily(int employeeId, string fullName, DateTime date);
    List<EmployeeAttendance>? CheckEmployeeAttendanceMonthly(int employeeId, string fullName, DateTime month);
    List<EmployeeAttendance>? CheckEmployeeListAttendanceDaily(DateTime date);
}

