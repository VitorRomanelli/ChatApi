using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApi.Persistence.Migrations
{
    public partial class Adding_Visualized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisualizedRecipient",
                table: "Messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisualizedSender",
                table: "Messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisualizedRecipient",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "VisualizedSender",
                table: "Messages");
        }
    }
}
