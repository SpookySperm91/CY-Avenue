namespace CarlosYulo.backend.entities.class_session;

public class ClassSessionMembers
{
    public int? SessionId { get; set; }
    public int? MemberId { get; set; }
    public string? MemberName { get; set; }
    public int? TrainerId { get; set; }
    public string? TrainerName { get; set; }
    public int? SessionRoom { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
}