using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowData.Migrations
{
    public partial class AddImageIntoDataOverviewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "DataOverviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "DataOverviews");
        }
    }
}
