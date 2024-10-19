using System.Data;
using CarlosYulo.backend.entities.class_session;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.schedule.ss_delete;

public class ScheduleDeleteAllByDay
{
    private DatabaseConnection dbConnection;

    public ScheduleDeleteAllByDay(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }
    
    private string procedure(ClassSessionType type)
    {
        return type switch
        {
            ClassSessionType.PERSONAL => "prcPersonalSessionDeleteAllDayPrevious",
            ClassSessionType.FIXED => "prcFixedSessionDeleteAllDayPrevious",
            _ => "prcPersonalSessionDeleteAllDayPrevious"
        };
    }
    
    

    public bool DeleteAllByDay(ClassSessionType type, DateTime date, out string message)
    {
        if (date > DateTime.Now)
        {
            message = "Date cannot be in the future";
            return false;
        }

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
                command.Parameters.AddWithValue("p_date", date);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"{rowsAffected} Fixed session this {date.ToString("MM/dd")} deleted successfully";
                    return true;
                }
                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = $"Failed to delete fixed session on {date.ToString("MM/dd")}";
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