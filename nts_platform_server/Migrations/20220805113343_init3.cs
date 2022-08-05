using Microsoft.EntityFrameworkCore.Migrations;

namespace nts_platform_server.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "Week",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "userId",
                table: "Projects",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_userId",
                table: "Projects",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_userId",
                table: "Projects",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_userId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_userId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Month",
                table: "Week",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
