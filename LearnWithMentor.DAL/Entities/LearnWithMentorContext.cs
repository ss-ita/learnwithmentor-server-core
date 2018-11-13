using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnWithMentor.DAL.Entities
{
    public partial class LearnWithMentorContext : DbContext
    {
        public LearnWithMentorContext()
            //: base("name=LearnWithMentorContext")
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateUserReferences(modelBuilder);
            CreateCommentReferences(modelBuilder);
            CreateGroupReferences(modelBuilder);
            CreateMessageReferences(modelBuilder);
            CreatePlanReferences(modelBuilder);
            CreatePlanSuggestionReferences(modelBuilder);
            CreatePlanTaskReferences(modelBuilder);
            CreateRoleReferences(modelBuilder);
            CreateSectionReferences(modelBuilder);
            CreateTaskReferences(modelBuilder);
            CreateUserTaskReferences(modelBuilder);
            //CreateManyToManyReferences(modelBuilder);
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanTask> PlanTasks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<StudentTask> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<PlanSuggestion> PlanSuggestion { get; set; }
        public virtual DbSet<GroupPlanTask> GROUPS_PLANS_TASKS { get; set; }
        public virtual DbSet<UserRole> USERS_ROLES { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        /*public virtual int sp_Total_Ammount_of_Users(ObjectParameter total)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_Total_Ammount_of_Users", total);
        }*/

        private void CreateUserReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);
            modelBuilder.Entity<User>()
                .Property(user => user.Role)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.Role_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreateCommentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id);
            modelBuilder.Entity<Comment>()
                .Property(comment => comment.Creator)
                .IsRequired();
            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Creator)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.Create_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .Property(comment => comment.PlanTask)
                .IsRequired();
            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.PlanTask)
                .WithMany(planTask => planTask.Comments)
                .HasForeignKey(comment => comment.PlanTask_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreateGroupReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasKey(group => group.Id);
            modelBuilder.Entity<Group>()
                .Property(group => group.Mentor)
                .IsRequired();
            modelBuilder.Entity<Group>()
                .HasOne(group => group.Mentor)
                .WithMany(mentor => mentor.GroupMentor)
                .HasForeignKey(group => group.Mentor_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreateMessageReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id);
            modelBuilder.Entity<Message>()
                .Property(message => message.Creator)
                .IsRequired();
            modelBuilder.Entity<Message>()
                .HasOne(message => message.Creator)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.User_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Message>()
                .Property(message => message.UserTask)
                .IsRequired();
            modelBuilder.Entity<Message>()
                .HasOne(message => message.UserTask)
                .WithMany(userTask => userTask.Messages)
                .HasForeignKey(message => message.UserTask_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreatePlanReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasKey(plan => plan.Id);
            modelBuilder.Entity<Plan>()
                .Property(plan => plan.Creator)
                .IsRequired();
            modelBuilder.Entity<Plan>()
                .HasOne(plan => plan.Creator)
                .WithMany(creator => creator.PlansCreated)
                .HasForeignKey(plan => plan.Create_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Plan>()
                .HasOne(plan => plan.Modifier)
                .WithMany(modifier => modifier.PlansModified)
                .HasForeignKey(plans => plans.Mod_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreatePlanSuggestionReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanSuggestion>()
                .HasKey(planSug => planSug.Id);
            modelBuilder.Entity<PlanSuggestion>()
                .Property(planSug => planSug.Mentor)
                .IsRequired();
            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.Mentor)
                .WithMany(user => user.PlanSuggestionsMentor)
                .HasForeignKey(planSug => planSug.Mentor_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlanSuggestion>()
                .Property(planSug => planSug.User)
                .IsRequired();
            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.User)
                .WithMany(user => user.PlanSuggestionsStudent)
                .HasForeignKey(planSug => planSug.User_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlanSuggestion>()
                .Property(planSug => planSug.Plan)
                .IsRequired();
            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.Plan)
                .WithMany(plan => plan.PlanSuggestion)
                .HasForeignKey(planSug => planSug.Plan_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreatePlanTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanTask>()
                .HasKey(planTask => planTask.Id);
            modelBuilder.Entity<PlanTask>()
                .Property(planTask => planTask.Plans)
                .IsRequired();
            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Plans)
                .WithMany(plan => plan.PlanTasks)
                .HasForeignKey(planTask => planTask.Plan_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlanTask>()
                .Property(planTask => planTask.Tasks)
                .IsRequired();
            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Tasks)
                .WithMany(task => task.PlanTasks)
                .HasForeignKey(planTask => planTask.Task_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Sections)
                .WithMany(section => section.PlanTasks)
                .HasForeignKey(planTask => planTask.Section_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreateRoleReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(role => role.Id);
        }

        private void CreateSectionReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasKey(section => section.Id);
        }

        private void CreateTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentTask>()
                .HasKey(task => task.Id);
            modelBuilder.Entity<StudentTask>()
                .Property(task => task.Creator)
                .IsRequired();
            modelBuilder.Entity<StudentTask>()
                .HasOne(task => task.Creator)
                .WithMany(user => user.TasksCreated)
                .HasForeignKey(task => task.Create_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StudentTask>()
                .HasOne(task => task.Modifier)
                .WithMany(user => user.TasksModified)
                .HasForeignKey(task => task.Mod_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        private void CreateUserTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
                .HasKey(userTask => userTask.Id);
            modelBuilder.Entity<UserTask>()
                .Property(userTask => userTask.User)
                .IsRequired();
            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(userTask => userTask.User_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserTask>()
                .Property(userTask => userTask.Mentor)
                .IsRequired();
            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.Mentor)
                .WithMany(user => user.UserTaskMentor)
                .HasForeignKey(userTask => userTask.Mentor_Id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.PlanTask)
                .WithMany(planTask => planTask.UserTasks)
                .HasForeignKey(userTask => userTask.PlanTask_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }

        /*private void CreateManyToManyReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(user => user.Groups)
                .WithMany(group => group.Users)
                .Map(userGroups =>
                {
                    userGroups.MapLeftKey("UserId");
                    userGroups.MapRightKey("GroupId");
                    userGroups.ToTable("UserGroups");
                });

            modelBuilder.Entity<Group>()
                .HasMany(group => group.Plans)
                .WithMany(plan => plan.Groups)
                .Map(groupPlans =>
                {
                    groupPlans.MapLeftKey("GroupId");
                    groupPlans.MapRightKey("PlanId");
                    groupPlans.ToTable("GroupPlans");
                });
        }*/
    }
}
