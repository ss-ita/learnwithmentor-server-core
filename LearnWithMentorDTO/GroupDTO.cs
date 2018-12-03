
namespace LearnWithMentorDTO
{
    public class GroupDTO
    {
        public GroupDTO() { }

        public GroupDTO(int id, string name, int? mentorId, string mentorName)
        {
            Id = id;
            Name = name;
            MentorId = mentorId;
            MentorName = mentorName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? MentorId { get; set; }
        public string MentorName { get; set; }
    }
}
