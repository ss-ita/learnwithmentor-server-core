using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class TaskDiscussion
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TaskId { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
