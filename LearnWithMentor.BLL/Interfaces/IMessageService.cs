using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface IMessageService : IDisposableService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(int userTaskId);
        bool SendMessage(MessageDTO newMessage);
        Task<bool> UpdateIsReadStateAsync(int userTaskId, MessageDTO message);
    }
}
