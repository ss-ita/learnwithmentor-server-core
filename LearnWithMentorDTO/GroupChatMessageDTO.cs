using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    class GroupChatMessageDTO
    {
        public GroupChatMessageDTO(int messageId, int senderId, string text)
        {
            MessageId = messageId;
            SenderId = senderId;
            Text = text;
        }

        public int MessageId;
        public int SenderId;
        public string Text;
    }
}
