using CarlosYulo.backend.monolith.employee;

namespace CarlosYulo.backend.monolith;

public class EmployeeService
{
    //private ICreate<Employee> employeeCreate;
    private ISearch<Employee, int?> employeeSearch;
    private IUpdate<Employee> employeeUpdate;
    private IDelete<Employee> employeeDelete;
    private IAttendance<Employee> employeeAttendance;
    private IEmployeeAttendance iEmployeeAttendance;

    public EmployeeService(
        // ICreate<Employee> employeeCreate,
         ISearch<Employee, int?> employeeSearch
        // IUpdate<Employee> employeeUpdate,
        // IDelete<Employee> employeeDelete,
        // IAttendance<Employee> employeeAttendance,
        //IEmployeeAttendance iEmployeeAttendance
        )
    {
        // this.employeeCreate = employeeCreate;
         this.employeeSearch = employeeSearch;
        // this.employeeUpdate = employeeUpdate;
        // this.employeeDelete = employeeDelete;
        // this.employeeAttendance = employeeAttendance;
       // this.iEmployeeAttendance = iEmployeeAttendance;
    }
    
    // SEARCH
    public Employee? SearchEmployeeById(int employeeId, int? employeeTypeId)
    {
        string message;
        return employeeSearch.SearchById(employeeId, employeeTypeId, out message);
    }
    
    public Employee? SearchEmployeeByFullName(string fullName, int? employeeTypeId)
    {
        if (String.IsNullOrEmpty(fullName) || String.IsNullOrWhiteSpace(fullName))
        {
            MessageBox.Show("Error searching employee. Input are null.", "Error search", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return null;
        }
        
        
        string message;
        Employee? employee = employeeSearch.SearchByFullName(fullName.TrimEnd(), employeeTypeId, out message);

        if (employee == null)
        {
            MessageBox.Show(message, "Error search", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        return employee;
    }

    public List<Employee> SearchAllEmployee()
    {
        string type = "";
        return employeeSearch.SearchAll(type);
    }
}