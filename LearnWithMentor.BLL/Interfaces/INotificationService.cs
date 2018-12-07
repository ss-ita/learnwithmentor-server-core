using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface INotificationService:IDisposableService
    { 
        Task<NotificationDTO> GetCommentAsync(int commentId);
        
    }
}
