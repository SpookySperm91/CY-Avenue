using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop.i_revenue;

public class RevenueGenerateFinalReport
{
    private DatabaseConnection dbConnection;

    public RevenueGenerateFinalReport(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public FinalRevenueReport? GenerateFinalRevenueReport(DateTime month, out string message)
    {
        FinalRevenueReport revenueReport = null;
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcFinalRevenueMonthlyReport",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_date", month);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        dbConnection.transaction.Rollback();
                        dbConnection.transaction = null;
                        message = "No records found";
                        return null;
                    }

                    if (reader.Read())
                    {
                        revenueReport = new FinalRevenueReport
                        {
                            RevenueDate = reader.GetDateTime("revenue_date"),
                            OriginalRevenue = reader.GetDouble("original_revenue"),
                            TotalLiability = reader.GetDouble("liabilities"),
                            FinalRevenue = reader.GetDouble("final_revenue")
                        };
                    }
                }

                dbConnection.transaction.Commit();
                dbConnection.transaction = null;
                message = "Success. General Revenue Report for" + month.ToString("MM/yyyy");
                return revenueReport;
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
            message = e.Message; // Capture the exception message for output
            return null;
        }
    }
}