using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApi.Persistence.Migrations
{
    public partial class Changing_Visualized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisualizedRecipient",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "VisualizedSender",
                table: "Messages",
                newName: "Visualized");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Visualized",
                table: "Messages",
                newName: "VisualizedSender");

            migrationBuilder.AddColumn<bool>(
                name: "VisualizedRecipient",
                table: "Messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
