using LearnWithMentor.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class GroupPlan
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
