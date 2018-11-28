using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.HasKey(userTask => userTask.Id);

           builder.HasOne(userTask => userTask.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(userTask => userTask.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(userTask => userTask.Mentor)
                .WithMany(user => user.UserTaskMentor)
                .HasForeignKey(userTask => userTask.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(userTask => userTask.PlanTask)
                .WithMany(planTask => planTask.UserTasks)
                .HasForeignKey(userTask => userTask.PlanTask_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
