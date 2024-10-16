using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.salary;

public class EmployeeSalaryUpdate
{
    private DatabaseConnection dbConnection;

    public EmployeeSalaryUpdate(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public bool UpdateSalary(Employee employee, double salary, out string message)
    {
        employee.Salary = salary;

        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcEmployeeUpdateSalary",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateSalaryMap(command, employee);

                // execute query and set transaction to null after
                command.ExecuteNonQuery();

                dbConnection.transaction.Commit();
                dbConnection.transaction = null;
                Console.WriteLine("Updated employee salary to " + employee.Salary + " successfully.");
                message = $"Employee {employee.FullName} salary updated successfully to {employee.Salary}.";
                return true;
            }
        }
        catch (MySqlException sqlEx)
        {
            // Rollback on SQL error
            if (dbConnection.transaction != null && dbConnection.transaction.Connection != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            // Handle SQL specific error message
            Console.WriteLine("SQL Error: " + sqlEx.Message);
            message = "SQL Error: " + sqlEx.Message;
            return false;
        }
        catch (Exception e)
        {
            // Roll back if error occurs
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            // Handle general errors
            Console.WriteLine("Error: " + e.Message);
            message = "Error: " + e.Message;
            return false;
        }
    }

    private void UpdateSalaryMap(MySqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("p_employee_id", employee.EmployeeId);
        command.Parameters.AddWithValue("p_salary", employee.Salary);
    }
}