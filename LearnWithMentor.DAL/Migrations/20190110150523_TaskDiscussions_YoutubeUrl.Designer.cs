﻿// <auto-generated />
using System;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LearnWithMentor.DAL.Migrations
{
    [DbContext(typeof(LearnWithMentorContext))]
    [Migration("20190110150523_TaskDiscussions_YoutubeUrl")]
    partial class TaskDiscussions_YoutubeUrl
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Create_Date");

                    b.Property<int>("Create_Id");

                    b.Property<DateTime?>("Mod_Date");

                    b.Property<int>("PlanTask_Id");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("Create_Id");

                    b.HasIndex("PlanTask_Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Mentor_Id")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Mentor_Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.GroupChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId");

                    b.Property<string>("TextMessage");

                    b.Property<DateTime>("Time");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("GroupChatMessages");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.GroupPlan", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("PlanId");

                    b.HasKey("GroupId", "PlanId");

                    b.HasIndex("PlanId");

                    b.ToTable("GroupPlans");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsRead");

                    b.Property<DateTime?>("Send_Time");

                    b.Property<string>("Text");

                    b.Property<int>("UserTask_Id");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("UserTask_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("IsRead");

                    b.Property<string>("Text");

                    b.Property<string>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Create_Date");

                    b.Property<int>("Create_Id");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Image_Name");

                    b.Property<DateTime?>("Mod_Date");

                    b.Property<int?>("Mod_Id");

                    b.Property<string>("Name");

                    b.Property<bool>("Published");

                    b.HasKey("Id");

                    b.HasIndex("Create_Id");

                    b.HasIndex("Mod_Id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.PlanSuggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Mentor_Id");

                    b.Property<int>("Plan_Id");

                    b.Property<string>("Text");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("Mentor_Id");

                    b.HasIndex("Plan_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("PlanSuggestion");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.PlanTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Plan_Id");

                    b.Property<int?>("Priority");

                    b.Property<int?>("Section_Id");

                    b.Property<int>("Task_Id");

                    b.HasKey("Id");

                    b.HasIndex("Plan_Id");

                    b.HasIndex("Section_Id");

                    b.HasIndex("Task_Id");

                    b.ToTable("PlanTasks");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.StudentTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Create_Date");

                    b.Property<int>("Create_Id");

                    b.Property<string>("Description");

                    b.Property<DateTime?>("Mod_Date");

                    b.Property<int?>("Mod_Id");

                    b.Property<string>("Name");

                    b.Property<bool>("Private");

                    b.Property<string>("Youtube_Url");

                    b.HasKey("Id");

                    b.HasIndex("Create_Id");

                    b.HasIndex("Mod_Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.TaskDiscussion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DatePosted");

                    b.Property<int>("SenderId");

                    b.Property<int>("TaskId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("TaskDiscussions");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Blocked");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Image");

                    b.Property<string>("Image_Name");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("Role_Id");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("Role_Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.UserGroup", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.UserTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("End_Date");

                    b.Property<int>("Mentor_Id");

                    b.Property<int>("PlanTask_Id");

                    b.Property<DateTime?>("Propose_End_Date");

                    b.Property<string>("Result");

                    b.Property<string>("State");

                    b.Property<int>("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("Mentor_Id");

                    b.HasIndex("PlanTask_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Comment", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Creator")
                        .WithMany("Comments")
                        .HasForeignKey("Create_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.PlanTask", "PlanTask")
                        .WithMany("Comments")
                        .HasForeignKey("PlanTask_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Group", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Mentor")
                        .WithMany("GroupMentor")
                        .HasForeignKey("Mentor_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.GroupPlan", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Group", "Group")
                        .WithMany("GroupPlans")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.Plan", "Plan")
                        .WithMany("GroupPlans")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Message", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.UserTask", "UserTask")
                        .WithMany("Messages")
                        .HasForeignKey("UserTask_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Creator")
                        .WithMany("Messages")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Notification", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.Plan", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Creator")
                        .WithMany("PlansCreated")
                        .HasForeignKey("Create_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Modifier")
                        .WithMany("PlansModified")
                        .HasForeignKey("Mod_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.PlanSuggestion", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Mentor")
                        .WithMany("PlanSuggestionsMentor")
                        .HasForeignKey("Mentor_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.Plan", "Plan")
                        .WithMany("PlanSuggestion")
                        .HasForeignKey("Plan_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "User")
                        .WithMany("PlanSuggestionsStudent")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.PlanTask", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Plan", "Plans")
                        .WithMany("PlanTasks")
                        .HasForeignKey("Plan_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.Section", "Sections")
                        .WithMany("PlanTasks")
                        .HasForeignKey("Section_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.StudentTask", "Tasks")
                        .WithMany("PlanTasks")
                        .HasForeignKey("Task_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.StudentTask", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Creator")
                        .WithMany("TasksCreated")
                        .HasForeignKey("Create_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Modifier")
                        .WithMany("TasksModified")
                        .HasForeignKey("Mod_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.User", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.UserGroup", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("LearnWithMentor.DAL.Entities.UserTask", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User", "Mentor")
                        .WithMany("UserTaskMentor")
                        .HasForeignKey("Mentor_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.PlanTask", "PlanTask")
                        .WithMany("UserTasks")
                        .HasForeignKey("PlanTask_Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LearnWithMentor.DAL.Entities.User", "User")
                        .WithMany("UserTasks")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LearnWithMentor.DAL.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("LearnWithMentor.DAL.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
