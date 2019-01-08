using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentor.Models
{
    public class TaskDiscussionWithNames
    {
        public string FullName { get; set; }
        public TaskDiscussionDTO DiscussionMessage { get; set; }

        public TaskDiscussionWithNames(string fullName, TaskDiscussionDTO discussionMessage)
        {
            FullName = fullName;
            DiscussionMessage = discussionMessage;
        }
    }
}
