using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentorDTO
{
    class GroupChatDTO
    {
        public GroupChatDTO(int chatId, int groupId, IEnumerable<GroupChatMessageDTO> messages)
        {
            ChatId = chatId;
            GroupId = groupId;
            Messages = messages;
        }

        public int ChatId;
        public int GroupId;
        public IEnumerable<GroupChatMessageDTO> Messages;
    }
}
