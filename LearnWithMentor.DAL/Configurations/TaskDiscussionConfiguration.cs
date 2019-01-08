using System;
using System.Collections.Generic;
using System.Text;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnWithMentor.DAL.Configurations
{
    class TaskDiscussionConfiguration : IEntityTypeConfiguration<TaskDiscussion>
    {
        public void Configure(EntityTypeBuilder<TaskDiscussion> builder)
        {
            builder.HasKey(n => n.Id);
        }
    }
}
