using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowData.Migrations
{
    public partial class AddUserForTaskIntoTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserForTask",
                table: "tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserForTask",
                table: "tasks");
        }
    }
}
