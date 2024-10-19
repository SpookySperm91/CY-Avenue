namespace CarlosYulo.backend.entities.class_session;

public class ClassSession
{
        public int? SessionId { get; set; }
        public string? SessionType { get; set; }
        public string? SessionTitle { get; set; }
        public string? SessionDescription { get; set; }
        public int? SessionRoom { get; set; }
        public DateTime? SessionStartAt { get; set; }
        public DateTime? SessionEndAt { get; set; }
        public int? SessionRoomNumber { get; set; }
        
        // trainer
        public int? TrainerId { get; set; }
        public string? TrainerName { get; set; }
    
    
        public override string ToString()
        {
                return $"SessionId: {SessionId?.ToString() ?? "N/A"}, " +
                       $"SessionType: {SessionType ?? "N/A"}, " +
                       $"SessionTitle: {SessionTitle ?? "N/A"}, " +
                       $"SessionDescription: {SessionDescription ?? "N/A"}, " +
                       $"SessionRoom: {SessionRoom?.ToString() ?? "N/A"}, " +
                       $"SessionStartAt: {SessionStartAt?.ToString("g") ?? "N/A"}, " +
                       $"SessionEndAt: {SessionEndAt?.ToString("g") ?? "N/A"}, " +
                       $"SessionRoomNumber: {SessionRoomNumber?.ToString("g") ?? "N/A"}, " +
                       $"TrainerId: {TrainerId?.ToString() ?? "N/A"}, " +
                       $"TrainerName: {TrainerName ?? "N/A"}";
        }
}



public enum ClassSessionType {FIXED, PERSONAL}