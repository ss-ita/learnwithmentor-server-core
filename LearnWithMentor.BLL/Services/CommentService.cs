using System.Collections.Generic;
using LearnWithMentorDTO;
using LearnWithMentor.DAL.Entities;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentor.DAL.UnitOfWork;
using System.Threading.Tasks;

namespace LearnWithMentorBLL.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(IUnitOfWork db) : base(db)
        {
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            Comment comment = await db.Comments.GetAsync(commentId);
            if (comment == null)
            {
                return null;
            }
            var commentDTO = new  CommentDTO(comment.Id,
                                   comment.Text,
                                   comment.Create_Id,
                                   await db.Users.ExtractFullNameAsync(comment.Create_Id),
                                   comment.Create_Date,
                                   comment.Mod_Date);
            return  commentDTO;
        }

        public async Task<bool> AddCommentToPlanTaskAsync(int planTaskId, CommentDTO comment)
        {
            var plantask = await db.PlanTasks.Get(planTaskId);
            if (plantask == null)
            {
                return false;
            }
            if ( await db.Users.GetAsync(comment.CreatorId) == null)
            {
                return false;
            }
            var newComment = new Comment()
            {
                Text = comment.Text,
                PlanTask_Id = planTaskId,
                Create_Id = comment.CreatorId,
            };
            await db.Comments.AddAsync(newComment);
            db.Save();
            return true;
        }

        public async Task<bool> AddCommentToPlanTaskAsync(int planId, int taskId, CommentDTO comment)
        {
            var planTaskId = await db.PlanTasks.GetIdByTaskAndPlanAsync(taskId, planId);
            if (planTaskId == null)
            {
                return false;
            }
            return await AddCommentToPlanTaskAsync(planTaskId.Value, comment);
        }

        public async Task<bool> UpdateCommentIdTextAsync(int commentId, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            Comment comment = await db.Comments.GetAsync(commentId);
            if (comment == null)
            {
                return false;
            }
            comment.Text = text;
            await db.Comments.UpdateAsync(comment);
            db.Save();
            return true;
        }

        public async Task<bool> UpdateCommentAsync(int commentId, CommentDTO commentDTO)
        {
            if (commentDTO == null)
            {
                return false;
            }
            if (!await db.Comments.ContainsIdAsync(commentId))
            {
                return false;
            }
            return await UpdateCommentIdTextAsync(commentId, commentDTO.Text);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsForPlanTaskAsync(int taskId, int planId)
        {
            var planTaskId = await db.PlanTasks.GetIdByTaskAndPlanAsync(taskId, planId);
            return planTaskId == null ? null : await GetCommentsForPlanTaskAsync(planTaskId.Value);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsForPlanTaskAsync(int planTaskId)
        {
            var commentsList = new List<CommentDTO>();
            var planTask = await db.PlanTasks.Get(planTaskId);
            var comments = planTask?.Comments;
            if (comments == null)
            {
                return null;
            }
            foreach (var c in comments)
            {
                commentsList.Add(new CommentDTO(c.Id,
                                       c.Text,
                                       c.Create_Id,
                                       await db.Users.ExtractFullNameAsync(c.Create_Id),
                                       c.Create_Date,
                                       c.Mod_Date));
            }
            return commentsList;
        }

        public async Task<bool> RemoveByIdAsync(int commentId)
        {
            if (!await db.Comments.ContainsIdAsync(commentId))
            {
                return false;
            }
            db.Comments.RemoveById(commentId);
            db.Save();
            return true;
        }
    }
}
