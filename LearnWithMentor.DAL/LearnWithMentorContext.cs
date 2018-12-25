using Microsoft.EntityFrameworkCore;
using LearnWithMentor.DAL.Configurations;

namespace LearnWithMentor.DAL.Entities
{
    public partial class LearnWithMentorContext : DbContext
    {
        public LearnWithMentorContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region CallConfiguringMethods

            /*CreateUserReferences(modelBuilder);
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
<<<<<<< HEAD:LearnWithMentor.DAL/LearnWithMentorContext.cs
            CreateManyToManyReferences(modelBuilder);*/

            #endregion

            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new GroupPlanConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new PlanSuggestionConfiguration());
            modelBuilder.ApplyConfiguration(new PlanTaskConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new StudentTaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
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
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }                
        public virtual DbSet<GroupPlan> GroupPlans { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<GroupChatMessage> GroupChatMessages { get; set; }
    
        #region ConfiguringMethods

        /*private void CreateUserReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.Role_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }

        private void CreateCommentReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasKey(comment => comment.Id);

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Creator)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.PlanTask)
                .WithMany(planTask => planTask.Comments)
                .HasForeignKey(comment => comment.PlanTask_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }

        private void CreateGroupReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasKey(group => group.Id);

            modelBuilder.Entity<Group>()
                .HasOne(group => group.Mentor)
                .WithMany(mentor => mentor.GroupMentor)
                .HasForeignKey(group => group.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }

        private void CreateMessageReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id);

            modelBuilder.Entity<Message>()
                .HasOne(message => message.Creator)
                .WithMany(user => user.Messages)
                .HasForeignKey(message => message.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .HasOne(message => message.UserTask)
                .WithMany(userTask => userTask.Messages)
                .HasForeignKey(message => message.UserTask_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }

        private void CreatePlanReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasKey(plan => plan.Id);

            modelBuilder.Entity<Plan>()
                .HasOne(plan => plan.Creator)
                .WithMany(creator => creator.PlansCreated)
                .HasForeignKey(plan => plan.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Plan>()
                .HasOne(plan => plan.Modifier)
                .WithMany(modifier => modifier.PlansModified)
                .HasForeignKey(plans => plans.Mod_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void CreatePlanSuggestionReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanSuggestion>()
                .HasKey(planSug => planSug.Id);

            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.Mentor)
                .WithMany(user => user.PlanSuggestionsMentor)
                .HasForeignKey(planSug => planSug.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.User)
                .WithMany(user => user.PlanSuggestionsStudent)
                .HasForeignKey(planSug => planSug.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<PlanSuggestion>()
                .HasOne(planSug => planSug.Plan)
                .WithMany(plan => plan.PlanSuggestion)
                .HasForeignKey(planSug => planSug.Plan_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }

        private void CreatePlanTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlanTask>()
                .HasKey(planTask => planTask.Id);

            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Plans)
                .WithMany(plan => plan.PlanTasks)
                .HasForeignKey(planTask => planTask.Plan_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Tasks)
                .WithMany(task => task.PlanTasks)
                .HasForeignKey(planTask => planTask.Task_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<PlanTask>()
                .HasOne(planTask => planTask.Sections)
                .WithMany(section => section.PlanTasks)
                .HasForeignKey(planTask => planTask.Section_Id)
                .OnDelete(DeleteBehavior.Restrict);
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
                .HasOne(task => task.Creator)
                .WithMany(user => user.TasksCreated)
                .HasForeignKey(task => task.Create_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<StudentTask>()
                .HasOne(task => task.Modifier)
                .WithMany(user => user.TasksModified)
                .HasForeignKey(task => task.Mod_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void CreateUserTaskReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
                .HasKey(userTask => userTask.Id);

            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.User)
                .WithMany(user => user.UserTasks)
                .HasForeignKey(userTask => userTask.User_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.Mentor)
                .WithMany(user => user.UserTaskMentor)
                .HasForeignKey(userTask => userTask.Mentor_Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<UserTask>()
                .HasOne(userTask => userTask.PlanTask)
                .WithMany(planTask => planTask.UserTasks)
                .HasForeignKey(userTask => userTask.PlanTask_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void CreateNotificationReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().HasKey(n => n.Id);
        }

        private void CreateGroupMessageReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupChatMessage>().HasKey(n => n.Id);
        }

        private void CreateManyToManyReferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(s => new { s.GroupId, s.UserId });

            modelBuilder.Entity<UserGroup>()
                .HasOne<Group>(s => s.Group)
                .WithMany(s => s.UserGroups)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserGroup>()
                .HasOne<User>(s => s.User)
                .WithMany(s => s.UserGroups)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPlan>().HasKey(s => new { s.GroupId, s.PlanId });

            modelBuilder.Entity<GroupPlan>()
                .HasOne<Plan>(s => s.Plan)
                .WithMany(s => s.GroupPlans)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPlan>()
                .HasOne<Group>(s => s.Group)
                .WithMany(s => s.GroupPlans)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }*/

        #endregion
    }
}
