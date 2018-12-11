using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class GroupChatMessage
    {
        public int Message_Id { get; set; }
        public string TextMessage { get; set; }
        public int User_Id { get; set; }
        public int Group_Id { get; set; }  
        public DateTime Time { get; set; }
    }
}
