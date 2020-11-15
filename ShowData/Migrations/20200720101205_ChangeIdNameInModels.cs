using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowData.Migrations
{
    public partial class ChangeIdNameInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataOverviews",
                table: "DataOverviews");

            migrationBuilder.DropColumn(
                name: "ShowModelId",
                table: "ShowModels");

            migrationBuilder.DropColumn(
                name: "DataOverviewId",
                table: "DataOverviews");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShowModels",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DataOverviews",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataOverviews",
                table: "DataOverviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels",
                column: "DataOverviewId",
                principalTable: "DataOverviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataOverviews",
                table: "DataOverviews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShowModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DataOverviews");

            migrationBuilder.AddColumn<int>(
                name: "ShowModelId",
                table: "ShowModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DataOverviewId",
                table: "DataOverviews",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels",
                column: "ShowModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataOverviews",
                table: "DataOverviews",
                column: "DataOverviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels",
                column: "DataOverviewId",
                principalTable: "DataOverviews",
                principalColumn: "DataOverviewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
