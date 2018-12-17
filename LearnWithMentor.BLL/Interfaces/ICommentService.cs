using System.Collections.Generic;
using System.Threading.Tasks;
using LearnWithMentorDTO;

namespace LearnWithMentorBLL.Interfaces
{
    public interface ICommentService: IDisposableService
    {
        Task<CommentDTO> GetCommentAsync(int commentId);
        Task<IEnumerable<CommentDTO>> GetCommentsForPlanTaskAsync(int taskId, int planId);
        Task<IEnumerable<CommentDTO>> GetCommentsForPlanTaskAsync(int planTaskId);
        Task<bool> AddCommentToPlanTaskAsync(int planTaskId, CommentDTO comment);
        Task<bool> AddCommentToPlanTaskAsync(int planId, int taskId, CommentDTO comment);
        Task<bool> UpdateCommentIdTextAsync(int commentId, string text);
        Task<bool> UpdateCommentAsync(int commentId, CommentDTO commentDTO);
        Task<bool> RemoveByIdAsync(int commentId);
    }
}
