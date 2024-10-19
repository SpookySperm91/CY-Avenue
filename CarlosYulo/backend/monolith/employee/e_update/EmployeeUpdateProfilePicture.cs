using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.employee.update;

public class EmployeeUpdateProfilePicture 
{
    private DatabaseConnection dbConnection;

    public EmployeeUpdateProfilePicture(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private bool CheckCondition(Employee employee, string picturePath, out string message)
    {
        if (employee is null)
        {
            message = "Client is null";
            return false;
        }

        if (!File.Exists(picturePath))
        {
            message = "Picture file does not exist";
            return false;
        }

        message = string.Empty;
        return true;
    }

    public bool UpdateProfilePicture(Employee employee, string picturePath, out string message)
    {
        if (!CheckCondition(employee, picturePath, out message))
        {
            return false;
        }

        // set image path into readable and save in object
        if (!employee.SetProfilePicture(picturePath, out message))
        {
            return false;
        }
        
        // START OPERATION
        try
        {
            // begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcEmployeeUpdateProfilePicture",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                UpdateProfileProfileMap(command, employee);

                // execute query and set transaction to null after
                command.ExecuteNonQuery();
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    Console.WriteLine("Updated employee profile successfully.");
                    message = $"Employee {employee.FullName} profile picture updated successfully.";
                    return true;
                }
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

    private void UpdateProfileProfileMap(MySqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("p_employee_id", employee.EmployeeId);
        command.Parameters.AddWithValue("p_profile_pic", employee.ProfilePictureByte);
    }
}