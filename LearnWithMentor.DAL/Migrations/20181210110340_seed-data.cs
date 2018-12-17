using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace LearnWithMentor.DAL.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "LearnWithMentorDB", "Scripts", "Inserting_data_script.sql");
            migrationBuilder.Sql(File.ReadAllText(path));   
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
