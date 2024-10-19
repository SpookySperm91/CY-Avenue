using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.employee.create;
using CarlosYulo.backend.monolith.employee.delete;
using CarlosYulo.backend.monolith.employee.salary;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeOtherServices
{
    private readonly EmployeeCreateNew _employeeCreateNew;
    private readonly EmployeeDelete _employeeDelete;
    private readonly EmployeeSalaryUpdate _employeeSalaryUpdate;
    private readonly ErrorMessageBox _errorMessageBox;
    
    public EmployeeOtherServices(
        EmployeeCreateNew employeeCreateNew, 
        EmployeeDelete employeeDelete, 
        EmployeeSalaryUpdate employeeSalaryUpdate)
    {
        _employeeCreateNew = employeeCreateNew;
        _employeeDelete = employeeDelete;
        _employeeSalaryUpdate = employeeSalaryUpdate;
        _errorMessageBox = new ErrorMessageBox();
    }

    public bool Create(Employee employee)
    {
        string message;
        if (!_employeeCreateNew.Create(employee, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public bool DeleteEntity(Employee client)
    {
        string message;
        if (!_employeeDelete.DeleteEntity(client, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
    
    public bool DeleteById(int employeeId)
    {
        string message;
        if (!_employeeDelete.DeleteById(employeeId, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }


    public bool UpdateSalary(Employee employee, double salary)
    {
        string message;
        if (!_employeeSalaryUpdate.UpdateSalary(employee, salary, out message))
        {
            _errorMessageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    
    
    
    
    
        
}