using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnWithMentor.DAL.Entities;

namespace LearnWithMentor.DAL.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(plan => plan.Id);

            builder.HasOne(plan => plan.Creator)
                .WithMany(creator => creator.PlansCreated)
                .HasForeignKey(plan => plan.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(plan => plan.Modifier)
                .WithMany(modifier => modifier.PlansModified)
                .HasForeignKey(plans => plans.Mod_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
