using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(group => group.Id);

            builder.HasOne(group => group.Mentor)
                .WithMany(mentor => mentor.GroupMentor)
                .HasForeignKey(group => group.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
