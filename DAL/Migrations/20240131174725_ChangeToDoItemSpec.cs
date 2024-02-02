using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToDoItemSpec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "ToDoItems",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CompletionTime",
                table: "ToDoItems",
                newName: "CompletionDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "ToDoItems",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "ToDoItems",
                newName: "CompletionTime");
        }
    }
}
