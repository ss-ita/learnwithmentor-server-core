using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    public class TaskDiscussionDTO
    {
        public TaskDiscussionDTO(int id, int senderId, int taskId, string text, DateTime datePosted)
        {
            Id = id;
            SenderId = senderId;
            TaskId = taskId;
            Text = text;
            DatePosted = datePosted;
        }

        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TaskId { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
