using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee.search;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeSearchServices
{
    private readonly EmployeeSearchAll _searchAll;
    private readonly EmployeeSearchById _searchById;
    private readonly EmployeeSearchByName _searchByName;
    private readonly ErrorMessageBox _errorMessageBox;


    public EmployeeSearchServices(
        EmployeeSearchAll searchAll,
        EmployeeSearchById searchById,
        EmployeeSearchByName searchByName)
    {
        _searchAll = searchAll;
        _searchById = searchById;
        _searchByName = searchByName;
        _errorMessageBox = new ErrorMessageBox();
    }

    
    public List<Employee> SearchAll(EmployeTypeEnum? type)
    {
        return _searchAll.SearchAll(type);
    }

    
    public List<Employee>? SearchByFullName(string fullName)
    {
        string message;
        var employee = _searchByName.SearchByFullName(fullName, null, out message);
        if (employee is null || !employee.Any())
        {
            _errorMessageBox.ShowErrorMessage(message);
            return null;
        }

        return employee;
    }


    public Employee? SearchById(int employeeId, int? employeeType)
    {
        string message;
        var employee = _searchById.SearchById(employeeId, employeeType, out message);
        if (employee is null)
        {
            _errorMessageBox.ShowErrorMessage(message);
            return null;
        }

        return employee;
    }
}