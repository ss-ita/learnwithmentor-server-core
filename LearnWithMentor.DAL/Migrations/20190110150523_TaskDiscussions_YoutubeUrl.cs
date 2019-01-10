using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnWithMentor.DAL.Migrations
{
    public partial class TaskDiscussions_YoutubeUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Youtube_Url",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskDiscussions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    DatePosted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDiscussions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDiscussions");

            migrationBuilder.DropColumn(
                name: "Youtube_Url",
                table: "Tasks");
        }
    }
}
