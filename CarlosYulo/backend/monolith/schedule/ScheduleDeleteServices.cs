using CarlosYulo.backend.entities.class_session;
using CarlosYulo.backend.monolith.schedule.ss_delete;

namespace CarlosYulo.backend.monolith.schedule;

public class ScheduleDeleteServices
{
    private readonly ScheduleDeleteAllByDay _deleteByDay;
    private readonly ScheduleDeleteAllPrevious _deleteAllPrevious;


    public ScheduleDeleteServices(
        ScheduleDeleteAllByDay deleteByDay,
        ScheduleDeleteAllPrevious deleteAllPrevious)
    {
        _deleteByDay = deleteByDay;
        _deleteAllPrevious = deleteAllPrevious;
    }


    public bool DeleteAllByDay(ClassSessionType type, DateTime date)
    {
        string message;
        if (!_deleteByDay.DeleteAllByDay(type, date, out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        return true;
    }

    public bool DeleteAllPrevious(ClassSessionType type)
    {
        string message;
        if (!_deleteAllPrevious.DeleteAllPrevious(type, out message))
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        return true;
    }
    
}