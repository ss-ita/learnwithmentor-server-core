using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public class UserRole
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Roles_Name { get; set; }
        public string Email { get; set; }
    }
}
