using Microsoft.EntityFrameworkCore.Migrations;

namespace LearnWithMentor.DAL.Migrations
{
    public partial class AddCommunication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TaskDiscussions_TaskId",
                table: "TaskDiscussions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMessages_GroupId",
                table: "GroupChatMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMessages_UserId",
                table: "GroupChatMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessages_Groups_GroupId",
                table: "GroupChatMessages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMessages_AspNetUsers_UserId",
                table: "GroupChatMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDiscussions_Tasks_TaskId",
                table: "TaskDiscussions",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessages_Groups_GroupId",
                table: "GroupChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMessages_AspNetUsers_UserId",
                table: "GroupChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskDiscussions_Tasks_TaskId",
                table: "TaskDiscussions");

            migrationBuilder.DropIndex(
                name: "IX_TaskDiscussions_TaskId",
                table: "TaskDiscussions");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatMessages_GroupId",
                table: "GroupChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatMessages_UserId",
                table: "GroupChatMessages");
        }
    }
}
