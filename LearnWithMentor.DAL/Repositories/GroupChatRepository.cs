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
    public class GroupChatRepository : BaseRepository<GroupChatMessage>, IGroupChatRepository
    {
        public GroupChatRepository(LearnWithMentorContext context) : base(context)
        {

        }

        public async Task AddMessageAsync(GroupChatMessage message)
        {
            try
            {
                await Context.AddAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<IEnumerable<GroupChatMessage>> GetGroupMessagesAsync(int groupId)
        {
            return await Context.GroupChatMessages
                .Where(groupChatMessage => groupChatMessage.GroupId == groupId)
                .OrderBy(groupChatMessage => groupChatMessage.Time)
                .ToListAsync();
        }

        public async Task<IEnumerable<GroupChatMessage>> GetGroupMessagesAsync(int groupId, int amount)
        {
            return await Context.GroupChatMessages
                .Where(groupChatMessage => groupChatMessage.GroupId == groupId)
                .OrderByDescending(groupChatMessage => groupChatMessage.Time)
                .Take(amount)
                .ToListAsync();
        }
    }
}
