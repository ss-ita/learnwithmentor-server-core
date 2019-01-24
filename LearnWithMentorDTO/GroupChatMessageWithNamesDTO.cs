using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    public class GroupChatMessageWithNamesDTO
    {
        public int SenderId;
        public string FullName;
        public string TextMessage;
        public string Time;

        public GroupChatMessageWithNamesDTO(int senderId, string fullName, string textMessage, string time)
        {
            SenderId = senderId;
            FullName = fullName;
            TextMessage = textMessage;
            Time = time;
        }
    }
}
