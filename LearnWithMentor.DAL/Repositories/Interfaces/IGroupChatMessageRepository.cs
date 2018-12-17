using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface IGroupChatMessageRepository
    {
        Task AddMessageAsync(GroupChatMessage message);
        Task<IEnumerable<GroupChatMessage>> GetGroupMessagesAsync(int groupId);
    }
}
