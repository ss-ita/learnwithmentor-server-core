using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class GroupChatMessage
    {
        public int Id { get; set; }
        public string TextMessage { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }  
        public DateTime TimeSent { get; set; }
    }
}
