using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LearnWithMentor.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            GroupMentor = new HashSet<Group>();
            PlansCreated = new HashSet<Plan>();
            PlansModified = new HashSet<Plan>();
            TasksCreated = new HashSet<StudentTask>();
            TasksModified = new HashSet<StudentTask>();
            PlanSuggestionsStudent = new HashSet<PlanSuggestion>();
            PlanSuggestionsMentor = new HashSet<PlanSuggestion>();
            UserTasks = new HashSet<UserTask>();
            UserGroups = new HashSet<UserGroup>();
            UserTaskMentor = new HashSet<UserTask>();
            Messages = new HashSet<Message>();
        }

        //public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        public bool Blocked { get; set; }
        public string Image { get; set; }
        public string Image_Name { get; set; }
        //public bool EmailConfirmed { get; set; }
        public int Role_Id { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Group> GroupMentor { get; set; }
        public virtual ICollection<Plan> PlansCreated { get; set; }
        public virtual ICollection<Plan> PlansModified { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<StudentTask> TasksCreated { get; set; }
        public virtual ICollection<StudentTask> TasksModified { get; set; }
        public virtual ICollection<PlanSuggestion> PlanSuggestionsStudent { get; set; }
        public virtual ICollection<PlanSuggestion> PlanSuggestionsMentor { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserTask> UserTaskMentor { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
