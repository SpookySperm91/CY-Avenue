using System.Data;
using CarlosYulo.backend.entities.class_session;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.schedule.ss_delete;

public class ScheduleDeleteAllPrevious
{
    private DatabaseConnection dbConnection;

    public ScheduleDeleteAllPrevious(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    private string procedure(ClassSessionType type)
    {
        return type switch
        {
            ClassSessionType.PERSONAL => "prcPersonalSessionDeleteAllPrevious",
            ClassSessionType.FIXED => "prcFixedSessionDeleteAllPrevious",
            _ => "prcFixedSessionDeleteAllPrevious"
        };
    }

    public bool DeleteAllPrevious(ClassSessionType type, out string message)
    {
        string prc = procedure(type);

        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand(prc,
                       dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"{rowsAffected} Fixed session deleted successfully";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Failed to delete fixed sessions";
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