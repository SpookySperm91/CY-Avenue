using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.delete;

public class EmployeeDelete 
{
    private DatabaseConnection dbConnection;

    public EmployeeDelete(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool IsValidEmployeeId(int? membershipId)
    {
        return membershipId.HasValue && membershipId.Value >= 100000 && membershipId.Value <= 999999;
    }

    public bool DeleteEntity(Employee? client, out string message)
    {
        if (client is null)
        {
            message = "Client object is null.";
            return false;
        }

        if (client.EmployeeId == null)
        {
            message = "Client's Membership ID is null.";
            return false;
        }

        // Use the actual membership ID from the client object
        return DeleteOperation(client.EmployeeId, out message);
    }


    public bool DeleteById(int employeeId, out string message)
    {
        if (employeeId < 100000 || employeeId > 999999)
        {
            message = "Invalid membership ID input. Please try again. Members ID must be 6 digits.";
            return false;
        }

        // helper class
        return DeleteOperation(employeeId, out message);
    }

    private bool DeleteOperation(int? employeeId, out string message)
    {
        if (!IsValidEmployeeId(employeeId))
        {
            message = "Invalid Employee-ID input. Please try again. Employee-ID must be 6 digits.";
            return false;
        }
        
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcEmployeeDeleteByEmployeeId",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_employee_id", employeeId);

                // Add output parameter for rows deleted
                MySqlParameter rowsDeletedParam = new MySqlParameter("p_rows_deleted", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rowsDeletedParam);

                // Execute the query
                command.ExecuteNonQuery();

                int rowsDeleted = Convert.ToInt32(rowsDeletedParam.Value);

                if (rowsDeleted > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"Employee with ID: {employeeId} successfully deleted.";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"No employee found with ID: {employeeId}. Deletion failed.";
                return false;
            }
        }

        catch (Exception e)
        {
            // Roll back if error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = e.Message;
            return false;
        }
    }
}