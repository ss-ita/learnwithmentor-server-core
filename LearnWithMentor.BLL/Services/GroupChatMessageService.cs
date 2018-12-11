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
    public class GroupChatMessageService : BaseService, IGroupChatMesssageService
    {
        public GroupChatMessageService(IUnitOfWork db) : base(db)
        {

        }

        public async Task AddGroupChatMessageAsync(int userId, int groupId, string text, DateTime timeSent)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var groupChatMessage = new GroupChatMessage
            {
                Group_Id = groupId,
                User_Id = userId,
                TextMessage = text,
                Time = timeSent
            };
            await db.GroupChatMessage.AddMessageAsync(groupChatMessage);
            db.Save();
        }

        public async Task<IEnumerable<GroupChatMessageDTO>> GetGroupMessagesAsync(int groupId)
        {
            var groupChatMessages = await db.GroupChatMessage.GetGroupMessagesAsync(groupId);
            var groupChatMessagesDtoList = groupChatMessages.Select(n =>
                new GroupChatMessageDTO(
                    n.Group_Id,
                    n.Message_Id,
                    n.TextMessage,
                    n.Time)

            );
            return groupChatMessagesDtoList;

        }
    }
}
