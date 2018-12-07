using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public virtual User Recipient { get; set; }
    }
}
