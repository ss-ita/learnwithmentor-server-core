using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }

        public NotificationDTO(
            int id,
            int userId,
            bool isRead,
            string text,
            string status)    
        {
            Id = id;
            UserId = userId;
            IsRead = isRead;
            Text = text;
            Status = status;
        }
}
}
