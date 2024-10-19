using CarlosYulo.backend.entities.class_session;
using CarlosYulo.backend.monolith.common;
using CarlosYulo.backend.monolith.session.ss_create;

namespace CarlosYulo.backend.monolith.schedule;

public class ScheduleCreateServices
{
    private readonly ScheduleCreateFixed _fixed;
    private readonly ScheduleCreatePersonalClassMembers _classMembers;
    private readonly ScheduleCreatePersonal _personal;
    private readonly ErrorMessageBox _messageBox;

    public ScheduleCreateServices(
        ScheduleCreateFixed scheduleCreateFixed,
        ScheduleCreatePersonalClassMembers classMembers,
        ScheduleCreatePersonal personal)
    {
        _fixed = scheduleCreateFixed;
        _classMembers = classMembers;
        _personal = personal;
        _messageBox = new ErrorMessageBox();
    }

    public bool CreateClassSession(ClassSession? classSession)
    {
        string message;
        if (!_fixed.CreateClassSession(classSession, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }

    public List<ClassSessionMembers>? CreateSchedulePersonalClassMembers(int? sessionId)
    {
        string message;
        var classStudent = _classMembers.CreateSchedulePersonalClassMembers(sessionId, out message);

        if (classStudent is null)
        {
            _messageBox.ShowErrorMessage(message);
            return null;
        }

        return classStudent;
    }

    public bool CreatePersonalClassSession(ClassSession? classSession)
    {
        string message;
        if (!_personal.CreatePersonalClassSession(classSession, out message))
        {
            _messageBox.ShowErrorMessage(message);
            return false;
        }

        return true;
    }
}