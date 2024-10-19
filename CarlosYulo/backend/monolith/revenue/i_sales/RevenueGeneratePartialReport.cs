using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop.i_revenue;

public class RevenueGeneratePartialReport
{
    private DatabaseConnection dbConnection;

    public RevenueGeneratePartialReport(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public PartialRevenueReport? CreatePartialRevenueReport(DateTime date, out string message)
    {
        PartialRevenueReport revenueReport = null;
        message = string.Empty; // Initialize message

        try
        {
            // Begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcRevenuePartialReportCreate",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_date", date);

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
                        revenueReport = new PartialRevenueReport
                        {
                            RevenueDate = reader.GetDateTime("revenue_date"),
                            ItemSales = reader.GetDouble("item_sales"),
                            MembershipSales = reader.GetDouble("membership_sales"),
                            TotalRevenue = reader.GetDouble("total_revenue")
                        };
                    }
                }
                
                dbConnection.transaction.Commit();
                dbConnection.transaction = null;
                message = "Success. General Revenue Report for" + date.ToString("MM/yyyy");
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