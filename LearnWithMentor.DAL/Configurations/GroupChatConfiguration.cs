using System;
using System.Collections.Generic;
using System.Text;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnWithMentor.DAL.Configurations
{
    class GroupChatConfiguration : IEntityTypeConfiguration<GroupChatMessage>
    {
        public void Configure(EntityTypeBuilder<GroupChatMessage> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(message => message.ChatUser)
                .WithMany(user => user.GroupChatMessages)
                .HasForeignKey(message => message.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            builder.HasOne(message => message.ChatGroup)
                .WithMany(group => group.GroupChatMessages)
                .HasForeignKey(message => message.ChatGroup)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
