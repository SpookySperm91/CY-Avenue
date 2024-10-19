using CarlosYulo.backend.entities.class_session;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.schedule.ss_search;

namespace CarlosYulo.backend.monolith.schedule;

public class ScheduleSearchServices
{
    private readonly ScheduleSearchAll _searchAll;
    private readonly ErrorMessageBox _messageBox;


    public ScheduleSearchServices(ScheduleSearchAll searchAll)
    {
        _searchAll = searchAll;
        _messageBox = new ErrorMessageBox();
    }

    public List<ClassSession>? CreateSchedulePersonalClassMembers(ClassSessionType type)
    {
        string message;
        var classSessions = _searchAll.CreateSchedulePersonalClassMembers(type, out message);
        if (classSessions is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return classSessions;
    }
}