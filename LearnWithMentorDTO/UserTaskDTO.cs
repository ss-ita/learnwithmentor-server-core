using System;

namespace LearnWithMentorDTO
{
    public class UserTaskDTO
    {
        public UserTaskDTO() { }
        public UserTaskDTO(
            int id,
            int userId,
            int planTaskId,
            DateTime? endDate,
            DateTime? proposeEndDate,
            int mentorId,
            string state = "P",
            string result = "")
        {
            Id = id;
            UserId = userId;
            PlanTaskId = planTaskId;
            State = state;
            EndDate = endDate;
            Result = result;
            ProposeEndDate = proposeEndDate;
            MentorId = mentorId;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanTaskId { get; set; }
        public string State { get; set; }
        public DateTime? EndDate { get; set; }
        public string Result { get; set; }
        public DateTime? ProposeEndDate { get; set; }
        public int MentorId { get; set; }
    }
}
