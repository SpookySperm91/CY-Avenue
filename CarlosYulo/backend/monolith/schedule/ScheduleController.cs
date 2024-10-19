using CarlosYulo.backend.entities.class_session;

namespace CarlosYulo.backend.monolith.schedule;

public class ScheduleController
{
    private readonly ScheduleCreateServices _create;
    private readonly ScheduleSearchServices _search;
    private readonly ScheduleDeleteServices _delete;

    public ScheduleController(
        ScheduleCreateServices create,
        ScheduleSearchServices search,
        ScheduleDeleteServices delete)
    {
        _create = create;
        _search = search;
        _delete = delete;
    }
    
    // CREATE
    public bool CreateClassSession(ClassSession? classSession)
    {
        return _create.CreateClassSession(classSession);
    }

    public List<ClassSessionMembers>? CreateSchedulePersonalClassMembers(int? sessionId)
    {
        return _create.CreateSchedulePersonalClassMembers(sessionId);
    }

    
    // SEARCH
    public List<ClassSession>? CreateSchedulePersonalClassMembers(ClassSessionType type)
    {
       return _search.CreateSchedulePersonalClassMembers(type);
    }
    
    
    // DELETE
    public bool DeleteAllByDay(ClassSessionType type, DateTime date)
    {
        return _delete.DeleteAllByDay(type, date);
    }

    public bool DeleteAllPrevious(ClassSessionType type)
    {
        return _delete.DeleteAllPrevious(type);
    }
}