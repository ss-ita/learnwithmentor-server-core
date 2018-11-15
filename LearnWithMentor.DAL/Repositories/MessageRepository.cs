using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(LearnWithMentorContext context) : base(context)
        {
        }

        public async Task<Message> GetAsync(int id)
        {
            Message findMessage = await Context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            return findMessage;
        }

        public async Task<IEnumerable<Message>> GetByUserTaskIdAsync(int userTaskId)
        {
            UserTask findUserTask = await Context.UserTasks.FirstOrDefaultAsync(userTask => userTask.Id == userTaskId);
            return findUserTask?.Messages;
        }

        public async Task<bool> SendForUserTaskIdAsync(int userTaskId, Message message)
        {
            UserTask findUserTask = await Context.UserTasks.FirstOrDefaultAsync(task => task.Id == userTaskId);
            if (findUserTask != null)
            {
                findUserTask.Messages.Add(message);
                return true;
            }
            return false;
        }
    }
}
