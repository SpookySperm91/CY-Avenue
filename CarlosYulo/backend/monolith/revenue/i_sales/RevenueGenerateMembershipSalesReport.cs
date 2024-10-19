using System.Data;
using CarlosYulo.backend.entities;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.shop.i_revenue;

public class RevenueGenerateMembershipSalesReport
{
    private DatabaseConnection dbConnection;

    public RevenueGenerateMembershipSalesReport(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public MembershipSale? GetMembershipSales(Client client, MembershipSaleType membershipSaleType, out string message)
    {
        if (client is null)
        {
            message = "Client is null";
            return null;
        }

        MembershipSale saleReport = null;
        try
        {
            // Begin transaction only if no existing transaction 
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcRevenueClientCreate",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                MembershipSaleMap(command, client, membershipSaleType);

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
                        saleReport = Map(reader);
                    }

                    reader.Close();
                }

                dbConnection.transaction.Commit();
                dbConnection.transaction = null;

                message = "Client search successful";
                return saleReport;
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
            message = e.Message;
            return null;
        }
    }


    private void MembershipSaleMap(MySqlCommand command, Client client, MembershipSaleType membershipSaleType)
    {
        double price = client.MembershipTypeId switch
        {
            1 => 850,
            2 => 1250,
            3 => 75,
            _ => 0
        };

        string status = membershipSaleType switch
        {
            MembershipSaleType.RENEW => "Renew",
            MembershipSaleType.UPGRADE => "Upgrade",
            MembershipSaleType.NEW_MEMBER => "New",
            MembershipSaleType.WALK_IN => "Walk-in",
            _ => "Unknown"
        };

        command.Parameters.AddWithValue("p_membership_id", client.MembershipId);
        command.Parameters.AddWithValue("p_member_name", client.FullName);
        command.Parameters.AddWithValue("p_membership_type", client.Membership);
        command.Parameters.AddWithValue("p_price", price);
        command.Parameters.AddWithValue("p_status", status);
    }

    private MembershipSale Map(MySqlDataReader reader)
    {
        return new MembershipSale()
        {
            member_id = reader.GetInt32("member_id"), // Assuming membership_id is INT
            member_name = reader.GetString("member_name"), // Assuming member_name is VARCHAR
            membership_type = reader.GetString("membership_type"), // Assuming membership_type is VARCHAR
            price = reader.GetDouble("price"), // Assuming price is DECIMAL
            status = reader.GetString("status"), // Assuming status is VARCHAR
            sold_at = reader.IsDBNull("sold_at") ? null : reader.GetDateTime("sold_at") // Handle nullable DateTime
        };
    }
}