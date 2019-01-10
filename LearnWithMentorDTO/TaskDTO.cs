﻿using System;

namespace LearnWithMentorDTO
{
    public class TaskDTO
    {
        public TaskDTO() { }
        public TaskDTO(
            int id,
            string name,
            string description,
            bool privateness,
            int createId,
            string creatorName,
            int? modId,
            string modifierName,
            DateTime? createDate,
            DateTime? modDate,
            int? priority,
            int? sectionId,
            int? planTaskId,
			string youtubeUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            Private = privateness;
            CreateDate = createDate;
            ModDate = modDate;
            CreatorId = createId;
            CreatorName = creatorName;
            ModifierId = modId;
            ModifierName = modifierName;
            Priority = priority;
            SectionId = sectionId;
            PlanTaskId = planTaskId;
			Youtube_Url = youtubeUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }      
        public int? ModifierId { get; set; }
        public string ModifierName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
        public int? Priority { get; set; }
        public int? SectionId { get; set; }
        public int? PlanTaskId { get; set; }
		public string Youtube_Url { get; set; }
    }
}
