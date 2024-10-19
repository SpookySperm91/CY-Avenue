using System.Data;
using CarlosYulo.backend.entities.class_session;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.session.ss_create;

public class ScheduleCreatePersonalClassMembers
{
    private DatabaseConnection dbConnection;

    public ScheduleCreatePersonalClassMembers(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public List<ClassSessionMembers>? CreateSchedulePersonalClassMembers(int? sessionId, out string message)
    {
        if (sessionId is null || sessionId <= 0 || sessionId.ToString().Length != 6)
        {
            message = "Session Id is invalid";
            return null;
        }

        List<ClassSessionMembers> sessionMembers = new List<ClassSessionMembers>();

        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcPersonalSessionSetMembers", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_session_id", sessionId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sessionMembers.Add(MapSessionMembers(reader));
                    }
                }
            }
            dbConnection.transaction.Commit();
            dbConnection.transaction = null;
            message = string.Empty;
            return sessionMembers;
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


    public ClassSessionMembers MapSessionMembers(MySqlDataReader reader)
    {
        return new ClassSessionMembers()
        {
            SessionId = reader.IsDBNull(reader.GetOrdinal("session_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("session_id")),
            MemberId = reader.IsDBNull(reader.GetOrdinal("member_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("member_id")),
            MemberName = reader.IsDBNull(reader.GetOrdinal("member_name"))
                ? null
                : reader.GetString(reader.GetOrdinal("member_name")),
            TrainerId = reader.IsDBNull(reader.GetOrdinal("trainer_id"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("trainer_id")),
            TrainerName = reader.IsDBNull(reader.GetOrdinal("trainer_name"))
                ? null
                : reader.GetString(reader.GetOrdinal("trainer_name")),
            SessionRoom = reader.IsDBNull(reader.GetOrdinal("room"))
                ? null
                : reader.GetInt32(reader.GetOrdinal("room")),
            StartAt = reader.IsDBNull(reader.GetOrdinal("start_at"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("start_at")),
            EndAt = reader.IsDBNull(reader.GetOrdinal("end_at"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("end_at"))
        };
    }
}