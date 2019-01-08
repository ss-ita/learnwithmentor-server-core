using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorDTO
{
    public class TaskDiscussionWithNamesDTO
    {
        public string FullName { get; set; }
        public TaskDiscussionDTO DiscussionMessage { get; set; }

        public TaskDiscussionWithNamesDTO(string fullName, TaskDiscussionDTO discussionMessage)
        {
            FullName = fullName;
            DiscussionMessage = discussionMessage;
        }
    }
}
