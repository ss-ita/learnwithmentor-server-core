using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnWithMentor.DAL.Migrations
{
    public partial class AddFieldToStudentTask2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Youtube_Url",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Youtube_Url",
                table: "Tasks");
        }
    }
}
