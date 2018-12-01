using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(message => message.Id);

            builder.HasOne(message => message.Creator)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(message => message.UserTask)
                .WithMany(userTask => userTask.Messages)
                .HasForeignKey(message => message.UserTask_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
