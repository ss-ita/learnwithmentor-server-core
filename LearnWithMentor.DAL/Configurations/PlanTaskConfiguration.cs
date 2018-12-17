using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class PlanTaskConfiguration : IEntityTypeConfiguration<PlanTask>
    {
        public void Configure(EntityTypeBuilder<PlanTask> builder)
        {
            builder.HasKey(planTask => planTask.Id);

            builder.HasOne(planTask => planTask.Plans)
                .WithMany(plan => plan.PlanTasks)
                .HasForeignKey(planTask => planTask.Plan_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(planTask => planTask.Tasks)
                .WithMany(task => task.PlanTasks)
                .HasForeignKey(planTask => planTask.Task_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(planTask => planTask.Sections)
                .WithMany(section => section.PlanTasks)
                .HasForeignKey(planTask => planTask.Section_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
