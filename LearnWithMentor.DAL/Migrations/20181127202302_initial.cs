using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnWithMentor.DAL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Blocked = table.Column<bool>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Image_Name = table.Column<string>(nullable: true),
                    Email_Confirmed = table.Column<bool>(nullable: false),
                    Role_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Mentor_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_Mentor_Id",
                        column: x => x.Mentor_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Published = table.Column<bool>(nullable: false),
                    Create_Id = table.Column<int>(nullable: false),
                    Mod_Id = table.Column<int>(nullable: true),
                    Create_Date = table.Column<DateTime>(nullable: true),
                    Mod_Date = table.Column<DateTime>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Image_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_Users_Create_Id",
                        column: x => x.Create_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Users_Mod_Id",
                        column: x => x.Mod_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Private = table.Column<bool>(nullable: false),
                    Create_Id = table.Column<int>(nullable: false),
                    Mod_Id = table.Column<int>(nullable: true),
                    Create_Date = table.Column<DateTime>(nullable: true),
                    Mod_Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_Create_Id",
                        column: x => x.Create_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_Mod_Id",
                        column: x => x.Mod_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupPlans",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    PlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPlans", x => new { x.GroupId, x.PlanId });
                    table.ForeignKey(
                        name: "FK_GroupPlans_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupPlans_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanSuggestion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Plan_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<int>(nullable: false),
                    Mentor_Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSuggestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanSuggestion_Users_Mentor_Id",
                        column: x => x.Mentor_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSuggestion_Plans_Plan_Id",
                        column: x => x.Plan_Id,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanSuggestion_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Plan_Id = table.Column<int>(nullable: false),
                    Task_Id = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: true),
                    Section_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanTasks_Plans_Plan_Id",
                        column: x => x.Plan_Id,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTasks_Sections_Section_Id",
                        column: x => x.Section_Id,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanTasks_Tasks_Task_Id",
                        column: x => x.Task_Id,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanTask_Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Create_Id = table.Column<int>(nullable: false),
                    Create_Date = table.Column<DateTime>(nullable: true),
                    Mod_Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_Create_Id",
                        column: x => x.Create_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_PlanTasks_PlanTask_Id",
                        column: x => x.PlanTask_Id,
                        principalTable: "PlanTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<int>(nullable: false),
                    PlanTask_Id = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: true),
                    End_Date = table.Column<DateTime>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Propose_End_Date = table.Column<DateTime>(nullable: true),
                    Mentor_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_Mentor_Id",
                        column: x => x.Mentor_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTasks_PlanTasks_PlanTask_Id",
                        column: x => x.PlanTask_Id,
                        principalTable: "PlanTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserTask_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Send_Time = table.Column<DateTime>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_UserTasks_UserTask_Id",
                        column: x => x.UserTask_Id,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Create_Id",
                table: "Comments",
                column: "Create_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PlanTask_Id",
                table: "Comments",
                column: "PlanTask_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPlans_PlanId",
                table: "GroupPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Mentor_Id",
                table: "Groups",
                column: "Mentor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserTask_Id",
                table: "Messages",
                column: "UserTask_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_User_Id",
                table: "Messages",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Create_Id",
                table: "Plans",
                column: "Create_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_Mod_Id",
                table: "Plans",
                column: "Mod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSuggestion_Mentor_Id",
                table: "PlanSuggestion",
                column: "Mentor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSuggestion_Plan_Id",
                table: "PlanSuggestion",
                column: "Plan_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanSuggestion_User_Id",
                table: "PlanSuggestion",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTasks_Plan_Id",
                table: "PlanTasks",
                column: "Plan_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTasks_Section_Id",
                table: "PlanTasks",
                column: "Section_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanTasks_Task_Id",
                table: "PlanTasks",
                column: "Task_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Create_Id",
                table: "Tasks",
                column: "Create_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Mod_Id",
                table: "Tasks",
                column: "Mod_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_Id",
                table: "Users",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_Mentor_Id",
                table: "UserTasks",
                column: "Mentor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_PlanTask_Id",
                table: "UserTasks",
                column: "PlanTask_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_User_Id",
                table: "UserTasks",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GroupPlans");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "PlanSuggestion");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "PlanTasks");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
