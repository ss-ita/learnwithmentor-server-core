using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class GroupPlanConfiguration : IEntityTypeConfiguration<GroupPlan>
    {
        public void Configure(EntityTypeBuilder<GroupPlan> builder)
        {
            builder.HasKey(s => new { s.GroupId, s.PlanId });

            builder.HasOne(s => s.Plan)
                .WithMany(s => s.GroupPlans)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Group)
                .WithMany(s => s.GroupPlans)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
