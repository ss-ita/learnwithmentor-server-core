using System;

namespace LearnWithMentorDTO
{
    public class MessageDTO
    {
        public MessageDTO(int id,
                        int senderId,
                        int userTaskId,
                        string senderName,
                        string text,
                        DateTime? sendTime,
                        bool isRead)
        {
            Id = id;
            SenderId = senderId;
            UserTaskId = userTaskId;
            Text = text;
            SendTime = sendTime;
            SenderName = senderName;
            IsRead = isRead;
        }

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int UserTaskId { get; set; }
        public string SenderName { get; set; }
        public string Text { get; set; }
        public DateTime? SendTime { get; set; }
        public bool IsRead { get; set; }
    }
}
