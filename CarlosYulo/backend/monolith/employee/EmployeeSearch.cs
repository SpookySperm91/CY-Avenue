using CarlosYulo.database;

namespace CarlosYulo.backend.monolith.employee;

public class EmployeeSearch : ISearch<EmployeeAttendance>
{
    private DatabaseConnector dbConnector;

    public EmployeeSearch(DatabaseConnector dbConnector)
    {
        this.dbConnector = dbConnector;
    }
    
    public List<Client> SearchEmployeeAll()
    {
        return null;
    }

    public EmployeeAttendance? SearchById(int membershipId, string? gender)
    {
        return null;
    }

    public EmployeeAttendance? SearchByFullName(string fullName, string? gender)
    {
        return null;
    }
    
    public List<EmployeeAttendance> SearchAll()
    {
        return null;
    }
}