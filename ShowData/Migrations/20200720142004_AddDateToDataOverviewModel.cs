using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowData.Migrations
{
    public partial class AddDateToDataOverviewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SomeDate",
                table: "DataOverviews",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SomeDate",
                table: "DataOverviews");
        }
    }
}
