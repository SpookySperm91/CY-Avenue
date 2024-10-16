using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith;

public class ClientUpdateStatus
{
    private DatabaseConnection dbConnection;

    public ClientUpdateStatus(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public void UpdateAllMembershipsToExpired()
    {
        if (!UpdateStatus(out string message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    private bool UpdateStatus(out string message)
    {
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcClientUpdateMembershipStatus",
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                // number of rows updated
                MySqlParameter rowsDeletedParam = new MySqlParameter("updated_count", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rowsDeletedParam);

                // execute query
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                
                int rowsDeleted = Convert.ToInt32(rowsDeletedParam.Value);
                if (rowsDeleted > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"Successfully updated {rowsDeleted} records";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Updating expired members failed. No expired members were updated";
                return false;
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
            throw;
        }
    }
}