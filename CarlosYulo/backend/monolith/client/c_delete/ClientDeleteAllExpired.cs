using System.Data;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.delete;

public class ClientDeleteAllExpired
{
    private DatabaseConnection dbConnection;

    public ClientDeleteAllExpired(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public void DeleteWalkInAllExpired()
    {
        string message;
        if (!DeleteAllExpired("prcClientDeleteAllExpiredMembership", "Membership", out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // DELETE ALL EXPIRED MEMBERSHER
    public void DeleteMembershipAllExpired()
    {
        string message;
        if (!DeleteAllExpired("prcClientDeleteAllExpiredWalkIn", "Walk-in", out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    private bool DeleteAllExpired(string procedure, string type, out string message)
    {
        try
        {
            if (dbConnection.transaction is null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }


            using (MySqlCommand command = new MySqlCommand(procedure,
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                MySqlParameter rowsDeletedParam = new MySqlParameter("deleted_count", MySqlDbType.Int32)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rowsDeletedParam);

                command.ExecuteNonQuery();

                int rowsDeleted = Convert.ToInt32(rowsDeletedParam.Value);
                if (rowsDeleted > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"Expired {type} clients successfully deleted. Deleted {rowsDeleted} records.";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Deleting expired {type} clients failed. No expired client. No records deleted.";

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

            message = e.Message;
            return false;
        }
    }
}