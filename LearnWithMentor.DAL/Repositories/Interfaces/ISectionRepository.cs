using LearnWithMentor.DAL.Entities;
using System.Threading.Tasks;

namespace LearnWithMentor.DAL.Repositories.Interfaces
{
    public interface ISectionRepository : IRepository<Section>
    {
        Task<Section> GetAsync(int id);
    }
}
