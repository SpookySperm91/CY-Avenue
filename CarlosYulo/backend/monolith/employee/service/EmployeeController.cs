using CarlosYulo.backend.monolith.employee.search;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeController
{
    private readonly EmployeeOtherServices _others;
    private readonly EmployeeAttendanceServices _attendance;
    private readonly EmployeeSearchServices _search;
    private readonly EmployeeUpdateServices _update;

    public EmployeeController(
        EmployeeOtherServices others,
        EmployeeAttendanceServices attendance,
        EmployeeSearchServices search,
        EmployeeUpdateServices update)
    {
        _others = others;
        _attendance = attendance;
        _search = search;
        _update = update;
    }
    
    
    // OTHERS (CREATE, DELETE, UPDATE SALARY)
    public bool Create(Employee employee)
    {
        return _others.Create(employee);
    }

    public bool DeleteEntity(Employee client)
    {
        return _others.DeleteEntity(client);
    }

    public bool DeleteById(int employeeId)
    {
        return _others.DeleteById(employeeId);
    }

    public bool UpdateSalary(Employee employee, double salary)
    {
        return _others.UpdateSalary(employee, salary);
    }
    
    
    // ATTENDANCE
    public bool CreateEmployeeAttendance(int? employeeId, AttendanceStatus attendanceStatus)
    {
        return _attendance.CreateEmployeeAttendance(employeeId, attendanceStatus);
    }

    public EmployeeAttendance? SearchEmployeeDailyAttendance(int employeeId, DateTime attendanceDate)
    {
        return _attendance.SearchEmployeeDailyAttendance(employeeId, attendanceDate);
    }

    public List<EmployeeAttendance>? SearchEmployeeMonthlyAttendance(int employeeId, DateTime attendanceDate)
    {
        return _attendance.SearchEmployeeMonthlyAttendance(employeeId, attendanceDate);
    }

    public List<EmployeeAttendance> SearchAll(DateTime checkDate, AttendanceType? procedure)
    {
        return _attendance.SearchAll(checkDate, procedure);
    }
    
    
    // SEARCH
    public List<Employee> SearchAll(EmployeTypeEnum? type)
    {
        return _search.SearchAll(type);
    }

    public List<Employee>? SearchByFullName(string fullName)
    {
        return _search.SearchByFullName(fullName);
    }

    public Employee? SearchById(int employeeId, int? employeeType)
    {
        return _search.SearchById(employeeId, employeeType);
    }
    
    
    // UPDATE
    public bool UpdateDetails(Employee employee)
    {
        return _update.UpdateDetails(employee);
    }

    public bool UpdateProfilePicture(Employee employee, string picturePath)
    {
        return _update.UpdateProfilePicture(employee, picturePath);
    }

}