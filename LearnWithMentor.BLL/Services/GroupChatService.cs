using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using LearnWithMentorDTO;

namespace LearnWithMentor.BLL.Services
{
    public class GroupChatService : BaseService, IGroupChatService
    {
        public GroupChatService(IUnitOfWork db) : base(db)
        {

        }

        public async Task AddGroupChatMessageAsync(int userId, int groupId, string text, DateTime timeSent)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var groupChatMessage = new GroupChatMessage
            {
                GroupId = groupId,
                UserId = userId,
                TextMessage = text,
                TimeSent = timeSent
            };
            await db.GroupChatMessage.AddMessageAsync(groupChatMessage);
            db.Save();
        }

        public async Task<IEnumerable<GroupChatMessageDTO>> GetGroupMessagesAsync(int groupId)
        {
            var groupChatMessages = await db.GroupChatMessage.GetGroupMessagesAsync(groupId);
            var groupChatMessagesDtoList = groupChatMessages.Select(n =>
                new GroupChatMessageDTO(  
                    n.Id,
                    n.TextMessage,
                    n.UserId,
                    n.GroupId, 
                    n.TimeSent)
            );
            return groupChatMessagesDtoList;

        }

        public async Task<IEnumerable<GroupChatMessageDTO>> GetGroupMessagesAsync(int groupId, int amount)
        {
            var groupChatMessages = await db.GroupChatMessage.GetGroupMessagesAsync(groupId, amount);
            var groupChatMessagesDtoList = groupChatMessages.Select(n =>
                new GroupChatMessageDTO(
                    n.Id,
                    n.TextMessage,
                    n.UserId,
                    n.GroupId,
                    n.TimeSent)
            );
            return groupChatMessagesDtoList;
        }
    }
}
