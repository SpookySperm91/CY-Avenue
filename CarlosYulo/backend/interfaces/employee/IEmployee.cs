namespace CarlosYulo.backend.monolith.employee;

public interface IEmployee
{
}

public interface IEmployeeCreate
{
    bool CreateEmployee(Client client);
}


public interface IEmployeeAttendance
{
    bool CreateEmployeeAttendance(EmployeeAttendance employee);
    bool DeleteEmployeeAttendance(EmployeeAttendance employee, int employeeId, string fullName, DateTime date);
    bool UpdateEmployeeAttendance(EmployeeAttendance employee, int employeeId, string fullName, DateTime date);
    EmployeeAttendance? CheckEmployeeAttendanceDaily(int employeeId, string fullName, DateTime date);
    List<EmployeeAttendance>? CheckEmployeeAttendanceMonthly(int employeeId, string fullName, DateTime month);
    List<EmployeeAttendance>? CheckEmployeeListAttendanceDaily(DateTime date);
}

public interface IEmployeeUpdate
{
    bool UpdateEmployee(Client client);
    bool UpdateEmployeeProfilePicture(Client client, string image);
    bool UpdateEmployeeMembershipType(Client client, MembershipType membership);
}

public interface IEmployeeDelete
{
    bool DeleteEmployee(Client client);
    bool DeleteEmployeeByMembershipId(int membershipId);
}

public interface IEmployeeGenerate
{
    int GenerateEmployeeMembershipId(Client client);
}