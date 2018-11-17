using LearnWithMentor.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class GroupPlan
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
}
