using CarlosYulo.backend.monolith.employee;

namespace CarlosYulo.backend.monolith;

public class EmployeeService
{
    private ICreate<Employee> employeeCreate;
    private ISearch<Employee, int?> employeeSearch;
    private IUpdate<Employee> employeeUpdate;
    private IDelete<Employee> employeeDelete;
    private IAttendance<Employee> employeeAttendance;
    private IEmployeeAttendance iEmployeeAttendance;

    public EmployeeService(
        ICreate<Employee> employeeCreate,
        ISearch<Employee, int?> employeeSearch,
        IUpdate<Employee> employeeUpdate,
        IDelete<Employee> employeeDelete,
        IAttendance<Employee> employeeAttendance,
        IEmployeeAttendance iEmployeeAttendance)
    {
        this.employeeCreate = employeeCreate;
        this.employeeSearch = employeeSearch;
        this.employeeUpdate = employeeUpdate;
        this.employeeDelete = employeeDelete;
        this.employeeAttendance = employeeAttendance;
        this.iEmployeeAttendance = iEmployeeAttendance;
    }
    
    // SEARCH
    public Employee? SearchEmployeeById(int employeeId, int? employeeTypeId)
    {
        return employeeSearch.SearchById(employeeId, employeeTypeId);
    }
    
    public Employee? SearchEmployeeByFullName(string fullName, int? employeeTypeId)
    {
        return employeeSearch.SearchByFullName(fullName, employeeTypeId);
    }

    public List<Employee> SearchAllEmployee()
    {
        return employeeSearch.SearchAll();
    }
}