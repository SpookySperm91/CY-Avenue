using CarlosYulo.database;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeDeletes
{
    private DatabaseConnector dbConnector;

    public EmployeeDeletes(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }


    public bool Delete(Employee employee)
    {
        return DeleteClientFunction("prcEmployeeDeleteByEmployeeId", null, employee);
    }

    public bool DeleteById(int? employeeId)
    {
        return DeleteClientFunction("prcEmployeeDeleteByEmployeeId", employeeId, null);
    }
    
    private bool DeleteClientFunction(string procedure, int? employeeId,  Employee? employee){

        try
        {
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        
        
        
        return false;
    }
}