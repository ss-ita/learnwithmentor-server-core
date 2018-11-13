using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class SectionRepository : BaseRepository<Section>, ISectionRepository
    {
        public SectionRepository(LearnWithMentorContext context) : base(context) { }

        public Task<Section> GetAsync(int id)
        {
            return Context.Sections.FirstOrDefaultAsync(t => t.Id == id);

        }
    }
}
