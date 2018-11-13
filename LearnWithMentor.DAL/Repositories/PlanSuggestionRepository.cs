using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor.DAL.Repositories
{
    public class PlanSuggestionRepository : BaseRepository<PlanSuggestion>, IPlanSuggestionRepository
    {
        public PlanSuggestionRepository(LearnWithMentorContext context) : base(context)
        {
        }
    }
}
