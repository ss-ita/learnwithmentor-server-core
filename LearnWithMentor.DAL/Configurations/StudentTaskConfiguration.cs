using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class StudentTaskConfiguration : IEntityTypeConfiguration<StudentTask>
    {
        public void Configure(EntityTypeBuilder<StudentTask> builder)
        {
            builder.HasKey(task => task.Id);

            builder.HasOne(task => task.Creator)
                .WithMany(user => user.TasksCreated)
                .HasForeignKey(task => task.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(task => task.Modifier)
                .WithMany(user => user.TasksModified)
                .HasForeignKey(task => task.Mod_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
