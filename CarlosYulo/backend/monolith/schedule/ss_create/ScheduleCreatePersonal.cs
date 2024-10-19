using System.Data;
using CarlosYulo.backend.entities.class_session;
using CarlosYulo.database;
using MySql.Data.MySqlClient;

namespace CarlosYulo.backend.monolith.session.ss_create;

public class ScheduleCreatePersonal
{
    private DatabaseConnection dbConnection;

    public ScheduleCreatePersonal(DatabaseConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }


    public bool CreatePersonalClassSession(ClassSession? classSession, out string message)
    {
        if (classSession is null)
        {
            message = "Class session is null";
            return false;
        }

        List<string> missingFields = new List<string>();
        ValidateFields(classSession, missingFields);

        // Create error message
        if (missingFields.Count > 0)
        {
            message = "Please fill out the following missing fields: " + string.Join(", ", missingFields);
            return false;
        }


        try
        {
            if (dbConnection.transaction == null)
            {
                dbConnection.transaction = dbConnection.mysqlConnection.BeginTransaction();
            }

            using (MySqlCommand command = new MySqlCommand("prcPersonalSessionCreate", dbConnection.mysqlConnection,
                       dbConnection.transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                MySqlParameter outputId = new MySqlParameter("p_session_id", MySqlDbType.Int32)
                    { Direction = ParameterDirection.Output };

                command.Parameters.Add(outputId);
                NewScheduleFixedMap(command, classSession);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    classSession.SessionType = "Personal";
                    classSession.SessionId = Convert.ToInt32(outputId.Value);

                    dbConnection.transaction.Commit();
                    dbConnection.transaction = null;
                    message = $"New personal session class {classSession.SessionTitle} created successfully";
                    return true;
                }

                dbConnection.transaction.Rollback();
                dbConnection.transaction = null;
                message = "Failed to create new personal session class";
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


    private void NewScheduleFixedMap(MySqlCommand command, ClassSession classSession)
    {
        command.Parameters.AddWithValue("p_full_name", classSession.SessionTitle?.TrimEnd());
        command.Parameters.AddWithValue("p_session_description", classSession.SessionDescription?.TrimEnd());
        command.Parameters.AddWithValue("p_trainer_id", classSession.TrainerId);
        command.Parameters.AddWithValue("p_start_at", classSession.SessionStartAt);
        command.Parameters.AddWithValue("p_end_at", classSession.SessionEndAt);
        command.Parameters.AddWithValue("p_room", classSession.SessionRoomNumber);
    }


// Method to validate basic fields
    private void ValidateFields(ClassSession classSession, List<string> missingFields)
    {
        if (string.IsNullOrWhiteSpace(classSession.SessionTitle))
        {
            missingFields.Add("Session Title");
        }

        if (string.IsNullOrWhiteSpace(classSession.SessionDescription))
        {
            missingFields.Add("Description");
        }

        if (classSession.TrainerId == null)
        {
            missingFields.Add("Trainer ID");
        }

        if (string.IsNullOrWhiteSpace(classSession.TrainerName))
        {
            missingFields.Add("Trainer Name");
        }

        if (classSession.SessionRoomNumber <= 0 || classSession.SessionRoomNumber > 4)
        {
            missingFields.Add("Room Number");
        }

        if (classSession.SessionStartAt is null || classSession.SessionStartAt < DateTime.Now)
        {
            missingFields.Add("Start-at");
        }

        if (classSession.SessionStartAt is null || classSession.SessionStartAt < DateTime.Now)
        {
            missingFields.Add("End-at");
        }
    }
}