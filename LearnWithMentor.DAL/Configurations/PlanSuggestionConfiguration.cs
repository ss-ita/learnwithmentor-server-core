using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class PlanSuggestionConfiguration : IEntityTypeConfiguration<PlanSuggestion>
    {
        public void Configure(EntityTypeBuilder<PlanSuggestion> builder)
        {
            builder.HasKey(planSug => planSug.Id);

            builder.HasOne(planSug => planSug.Mentor)
                .WithMany(user => user.PlanSuggestionsMentor)
                .HasForeignKey(planSug => planSug.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(planSug => planSug.User)
                .WithMany(user => user.PlanSuggestionsStudent)
                .HasForeignKey(planSug => planSug.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(planSug => planSug.Plan)
                .WithMany(plan => plan.PlanSuggestion)
                .HasForeignKey(planSug => planSug.Plan_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
