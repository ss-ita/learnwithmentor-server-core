using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using Task = System.Threading.Tasks.Task;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> GetAsync(int id);
        Task<bool> ContainsIdAsync(int id);
        void RemoveById(int id);
        Task<IEnumerable<Comment>> GetByPlanTaskIdAsync(int ptId);
    }
}
