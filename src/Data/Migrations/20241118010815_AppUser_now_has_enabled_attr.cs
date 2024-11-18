using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taller1.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppUser_now_has_enabled_attr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "enabledUser",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enabledUser",
                table: "AspNetUsers");
        }
    }
}
