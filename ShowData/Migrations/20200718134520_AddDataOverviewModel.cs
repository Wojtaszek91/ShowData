using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowData.Migrations
{
    public partial class AddDataOverviewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShowModels");

            migrationBuilder.AddColumn<int>(
                name: "ShowModelId",
                table: "ShowModels",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DataOverviewId",
                table: "ShowModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels",
                column: "ShowModelId");

            migrationBuilder.CreateTable(
                name: "DataOverviews",
                columns: table => new
                {
                    DataOverviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataIncluded = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataOverviews", x => x.DataOverviewId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowModels_DataOverviewId",
                table: "ShowModels",
                column: "DataOverviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels",
                column: "DataOverviewId",
                principalTable: "DataOverviews",
                principalColumn: "DataOverviewId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowModels_DataOverviews_DataOverviewId",
                table: "ShowModels");

            migrationBuilder.DropTable(
                name: "DataOverviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels");

            migrationBuilder.DropIndex(
                name: "IX_ShowModels_DataOverviewId",
                table: "ShowModels");

            migrationBuilder.DropColumn(
                name: "ShowModelId",
                table: "ShowModels");

            migrationBuilder.DropColumn(
                name: "DataOverviewId",
                table: "ShowModels");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShowModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowModels",
                table: "ShowModels",
                column: "Id");
        }
    }
}
