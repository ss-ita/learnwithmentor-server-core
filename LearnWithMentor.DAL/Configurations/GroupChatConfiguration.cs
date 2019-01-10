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
        }
    }
}
