using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool isRead { get; set; }
        public string Text { get; set; }
        public enum Type
        {
            Approve,
            Reject,
            Done,
            Message
        }
        public virtual User Recipient { get; set; }
    }
}
