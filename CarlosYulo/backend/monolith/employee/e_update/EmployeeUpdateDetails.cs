using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.update;

public class EmployeeUpdateDetails 
{
    private DatabaseConnection dbConnection;

    public EmployeeUpdateDetails(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool checkCondtions(Employee employee, out string errorMessage)
    {
        // Return false if client is null
        if (employee is null)
        {
            Console.WriteLine("Client cannot be null.");
            errorMessage = "Client cannot be null.";
            return false;
        }

        errorMessage = "";
        return true;
    }

    public bool UpdateDetails(Employee employee, out string message)
    {
        if (!checkCondtions(employee, out message))
        {
            return false;
        }

        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            // encase with 'using' to ensure proper dispose
            using (MySqlCommand command = new MySqlCommand("prcEmployeeUpdateData", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateEmployeeDataMap(command, employee);

                // Execute the command
                int rowCount = command.ExecuteNonQuery();

                if (rowCount > 0)
                {
                    dbConnection.transaction.Commit(); // commit if update is successful
                    dbConnection.transaction = null; // always null transaction after query
                    Console.WriteLine($"Employee {employee.FullName} updated successfully.");
                    message = $"Employee {employee.FullName} updated successfully.";
                    return true;
                }
                // rollback and return false
                dbConnection.transaction.Rollback(); // commit if update is successful
                dbConnection.transaction = null; // always null transaction after query
                Console.WriteLine($"Fail to update {employee.FullName}.");
                message = $"Fail to update {employee.FullName}.";
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

    private void UpdateEmployeeDataMap(MySqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("p_employee_id", employee.EmployeeId);
        command.Parameters.AddWithValue("p_full_name", employee.FullName?.TrimEnd());
        command.Parameters.AddWithValue("p_salary", employee.Salary);
        command.Parameters.AddWithValue("p_email", employee.Email?.TrimEnd());
        command.Parameters.AddWithValue("p_phone_number", employee.PhoneNumber?.TrimEnd());
        command.Parameters.AddWithValue("p_gender", employee.Gender?.TrimEnd());
        command.Parameters.AddWithValue("p_age", employee.Age ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_birthday", employee.BirthDate?.Date ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("p_profile_pic", employee.ProfilePictureByte);
    }
}