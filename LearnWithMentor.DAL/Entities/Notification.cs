using System;

namespace LearnWithMentor.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User User { get; set; }
    }
}
