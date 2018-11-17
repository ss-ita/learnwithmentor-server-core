using LearnWithMentor.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
