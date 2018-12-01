using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(comment => comment.Id);

            builder.HasOne(comment => comment.Creator)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(comment => comment.PlanTask)
                .WithMany(planTask => planTask.Comments)
                .HasForeignKey(comment => comment.PlanTask_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
