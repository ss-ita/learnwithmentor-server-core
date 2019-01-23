using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    public class GroupChatMessageDTO
    {
        public int MessageId;
        public string TextMessage;
        public int SenderId;
        public int GroupId;
        public DateTime Time;

        public GroupChatMessageDTO(int messageId, string textMessage, int senderId, int groupId, DateTime dateTime)
        {
            MessageId = messageId;
            TextMessage = textMessage;
            SenderId = senderId;
            GroupId = groupId;
            Time = dateTime;
        }

        
    }
}
