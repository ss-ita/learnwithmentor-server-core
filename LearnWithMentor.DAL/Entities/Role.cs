using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LearnWithMentor.DAL.Entities
{
    public class Role : IdentityRole<int>
    { 
        public Role()
        {
            Users = new HashSet<User>();
        }

        public Role(string name) : base(name)
        {
            Users = new HashSet<User>();
        }

        //public int Id { get; set; }
        //public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
