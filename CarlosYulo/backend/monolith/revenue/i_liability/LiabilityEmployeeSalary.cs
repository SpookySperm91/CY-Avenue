using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.revenue.i_liability;

public class LiabilityEmployeeSalary
{
    private DatabaseConnection dbConnection;

    public LiabilityEmployeeSalary(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public List<EmployeeSalary>? GenerateEmployeeSalary(DateTime month, out string message)
    {
        List<EmployeeSalary> employeesSalary = new List<EmployeeSalary>();
        message = string.Empty; // Initialize message

        try
        {
            // Begin a new transaction if one does not already exist
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcLiabilityEmployeeSalary", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_date", month);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there are results
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        dbConnection.transaction.Rollback();
                        dbConnection.transaction = null;
                        message = "Error. No Salary for this month";
                        Console.WriteLine(message);
                        return null; // Return an empty list instead of null
                    }

                    // Retrieve rows and set in object
                    while (reader.Read())
                    {
                        employeesSalary.Add(MapEmployeeSalary(reader));
                    }
                    reader.Close();
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = "Employee Salary Created Successfully. Tell Them To Claim <3";
                    return employeesSalary;
                }
            }
        }
        catch (Exception e)
        {
            if (dbConnection.transaction != null)
            {
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
            }

            Console.WriteLine(e);
            message = e.Message; // You may want to log this or handle it differently
            return null; // Return an empty list instead of null
        }
    }


    private EmployeeSalary MapEmployeeSalary(MySqlDataReader reader)
    {
        return new EmployeeSalary()
        {
            EmployeeId = reader.GetInt32("employee_id"),
            EmployeeName = reader.GetString("employee_name"),
            EmployeeType = reader.GetString("employee_type"),
            Salary = reader.GetDouble("payment_amount"), // Updated to reflect the payment amount
            PostedSalaryDate = reader.GetDateTime("payment_date"),
            IsPayed = reader.GetBoolean("has_paid"),
        };
    }
}