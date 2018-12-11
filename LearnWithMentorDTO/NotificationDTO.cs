using System;

namespace LearnWithMentorDTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public NotificationType Type { get; set; }
        public string DateTime { get; set; }

        public NotificationDTO(
            int id,
            int userId,
            bool isRead,
            string text,
            NotificationType type,
            string dateTime)    
        {
            Id = id;
            UserId = userId;
            IsRead = isRead;
            Text = text;
            Type = type;
            DateTime = dateTime;
        }
    }

    public enum NotificationType
    {
        TaskApproved,
        TaskCompleted,
        TaskRejected,
        TaskReset,
        NewMessage
    }
}
