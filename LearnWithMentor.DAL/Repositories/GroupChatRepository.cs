using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearnWithMentor.DAL.Repositories
{
    public class GroupChatRepository : BaseRepository<GroupChatMessage>, IGroupChatMessageRepository
    {
        public GroupChatRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public async Task AddMessageAsync( GroupChatMessage message)
        {
            await Context.AddAsync(message);
        }

        public async Task<IEnumerable<GroupChatMessage>> GetGroupMessagesAsync(int groupId)
        {
            return await Context.GroupChatMessages
                .Where(groupChatMessage => groupChatMessage.GroupId == groupId)
                .OrderByDescending(groupChatMessage => groupChatMessage.Time)
                .ToListAsync();
        }
    }
}
