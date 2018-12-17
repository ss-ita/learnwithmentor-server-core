using System.Collections.Generic;
using LearnWithMentor.DAL.Entities;
using LearnWithMentorDTO;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class MessageService: BaseService, IMessageService
    {
        public MessageService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync(int userTaskId)
        {
            UserTask userTask = await db.UserTasks.GetAsync(userTaskId);
            if (userTask == null)
            {
                return null;
            }
            var messages = userTask.Messages;
            var messageDTOs = new List<MessageDTO>();
            foreach (var message in messages)
            {
                messageDTOs.Add(new MessageDTO(message.Id,
                                       message.User_Id,
                                       message.UserTask_Id,
                                       await db.Users.ExtractFullNameAsync(message.User_Id),
                                       message.Text,
                                       message.Send_Time,
                                       message.IsRead
                                       ));
            }
            return messageDTOs;
        }

        public bool SendMessage(MessageDTO newMessage)
        {
            var message = new Message()
            {
                User_Id = newMessage.SenderId,
                Text = newMessage.Text,
                UserTask_Id = newMessage.UserTaskId
            };
            db.Messages.AddAsync(message);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateIsReadStateAsync(int userTaskId, MessageDTO message)
        {
            Message GetMessage= await db.Messages.GetAsync(message.Id);
            if (GetMessage == null) return false;
             GetMessage.IsRead = message.IsRead;
            await db.Messages.UpdateAsync(GetMessage);
            db.Save();
            return true;
        }
    }
}
