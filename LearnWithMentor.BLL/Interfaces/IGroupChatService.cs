using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentor.BLL.Interfaces
{
    public interface IGroupChatService
    {
        Task AddGroupChatMessageAsync(int userId, int groupId, string text, DateTime timeSent);
        Task<IEnumerable<GroupChatMessageDTO>> GetGroupMessagesAsync(int groupId);
        Task<IEnumerable<GroupChatMessageDTO>> GetGroupMessagesAsync(int groupId, int amount);
    }
}
