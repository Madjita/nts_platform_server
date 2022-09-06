using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nts_platform_server.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_userId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Info",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Users",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Projects",
                newName: "EnginerCreaterId");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Projects",
                newName: "indexAdd");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "Projects",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_userId",
                table: "Projects",
                newName: "IX_Projects_EnginerCreaterId");

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PRFseries = table.Column<int>(type: "int", nullable: false),
                    PRFnumber = table.Column<int>(type: "int", nullable: false),
                    PRFdatetaked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PRFdateback = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PRFcode = table.Column<int>(type: "int", nullable: false),
                    PRFtaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRFplaceborned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRFplaceregistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRFplacelived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPnumber = table.Column<int>(type: "int", nullable: false),
                    IPdatetaked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPdateback = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPcode = table.Column<int>(type: "int", nullable: false),
                    IPtaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPplaceborned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ULMnumber = table.Column<int>(type: "int", nullable: false),
                    ULMdatetaked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ULMdateback = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ULMcode = table.Column<int>(type: "int", nullable: false),
                    ULMtaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ULMplaceborned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Snils = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INN = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_EnginerCreaterId",
                table: "Projects",
                column: "EnginerCreaterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_EnginerCreaterId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Users",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "indexAdd",
                table: "Projects",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "EnginerCreaterId",
                table: "Projects",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Projects",
                newName: "Descriptions");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_EnginerCreaterId",
                table: "Projects",
                newName: "IX_Projects_userId");

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_userId",
                table: "Projects",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
