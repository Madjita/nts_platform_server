using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nts_platform_server.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrfSeries = table.Column<int>(type: "int", nullable: false),
                    PrfNumber = table.Column<int>(type: "int", nullable: false),
                    PrfDateTaked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrfDateBack = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrfCode = table.Column<int>(type: "int", nullable: false),
                    PrfTaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrfPlaceBorned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrfPlaceRegistration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrfPlaceLived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpNumber = table.Column<int>(type: "int", nullable: false),
                    IpDateTaked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpDateBack = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpCode = table.Column<int>(type: "int", nullable: false),
                    IpTaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpPlaceBorned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UlmNumber = table.Column<int>(type: "int", nullable: false),
                    UlmDateTaked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UlmDateBack = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UlmCode = table.Column<int>(type: "int", nullable: false),
                    UlmTaked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UlmPlaceBorned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Snils = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inn = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    indexAdd = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Progress = table.Column<int>(type: "int", nullable: false),
                    Done = table.Column<bool>(type: "bit", nullable: false),
                    MaxHour = table.Column<int>(type: "int", nullable: false),
                    ActualHour = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnginerCreaterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_EnginerCreaterId",
                        column: x => x.EnginerCreaterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactProject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactProject_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactProject_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProject_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProject_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportCheck",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckBankPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckBankPhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserProjectId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillPhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TicketPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketPhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CheckPlane_BorderTicketPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckPlane_BorderTicketPhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BorderTicketPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BorderTicketPhotoByte = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCheck_UserProject_UserProjectId",
                        column: x => x.UserProjectId,
                        principalTable: "UserProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Week",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    NumberWeek = table.Column<int>(type: "int", nullable: false),
                    MoHourId = table.Column<int>(type: "int", nullable: true),
                    TuHourId = table.Column<int>(type: "int", nullable: true),
                    WeHourId = table.Column<int>(type: "int", nullable: true),
                    ThHourId = table.Column<int>(type: "int", nullable: true),
                    FrHourId = table.Column<int>(type: "int", nullable: true),
                    SaHourId = table.Column<int>(type: "int", nullable: true),
                    SuHourId = table.Column<int>(type: "int", nullable: true),
                    UserProjectId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Week", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Week_UserProject_UserProjectId",
                        column: x => x.UserProjectId,
                        principalTable: "UserProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Week_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocHour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weekday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectNumber = table.Column<int>(type: "int", nullable: false),
                    ActivityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityCodeTravel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraverTimeG = table.Column<float>(type: "real", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkingTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WTHour = table.Column<float>(type: "real", nullable: false),
                    TravelTimeC = table.Column<float>(type: "real", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeekId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocHour_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name", "Place" },
                values: new object[] { 1, "NTS", null });

            migrationBuilder.InsertData(
                table: "ReportCheck",
                columns: new[] { "Id", "BillPhotoByte", "BillPhotoName", "CheckBankPhotoByte", "CheckBankPhotoName", "Date", "Descriptions", "Discriminator", "UserProjectId", "Value" },
                values: new object[] { 3, null, "bill", null, null, null, null, "CheckHostel", null, 50 });

            migrationBuilder.InsertData(
                table: "ReportCheck",
                columns: new[] { "Id", "CheckPlane_BorderTicketPhotoByte", "CheckPlane_BorderTicketPhotoName", "CheckBankPhotoByte", "CheckBankPhotoName", "Date", "Descriptions", "Discriminator", "TicketPhotoByte", "TicketPhotoName", "UserProjectId", "Value" },
                values: new object[] { 1, null, null, null, null, null, null, "CheckPlane", null, "tiket", null, 100 });

            migrationBuilder.InsertData(
                table: "ReportCheck",
                columns: new[] { "Id", "BorderTicketPhotoByte", "BorderTicketPhotoName", "CheckBankPhotoByte", "CheckBankPhotoName", "Date", "Descriptions", "Discriminator", "UserProjectId", "Value" },
                values: new object[] { 2, null, "train", null, null, null, null, "CheckTrain", null, 70 });

            migrationBuilder.InsertData(
                table: "ReportCheck",
                columns: new[] { "Id", "CheckBankPhotoByte", "CheckBankPhotoName", "Date", "Descriptions", "Discriminator", "UserProjectId", "Value" },
                values: new object[] { 4, null, "bank", null, null, "ReportCheck", null, 1 });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "engineer" },
                    { 3, "guest" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "Email", "FirstName", "MiddleName", "Password", "ProfileId", "RoleId", "SecondName" },
                values: new object[] { 1, 1, "xok", "Сергей", "Юрьевич", "$2a$11$cQCIxNpsuGVheM.irxNiIOsWnLyzC7MTnTZOin1Z1c587WwDwtCIK", 1, 1, "Смоглюк" });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "Date", "Inn", "IpCode", "IpDateBack", "IpDateTaked", "IpNumber", "IpPlaceBorned", "IpTaked", "Phone", "PhotoByte", "PhotoName", "PrfCode", "PrfDateBack", "PrfDateTaked", "PrfNumber", "PrfPlaceBorned", "PrfPlaceLived", "PrfPlaceRegistration", "PrfSeries", "PrfTaked", "Sex", "Snils", "UlmCode", "UlmDateBack", "UlmDateTaked", "UlmNumber", "UlmPlaceBorned", "UlmTaked" },
                values: new object[] { 1, new DateTime(1994, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, 111, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, "Гор. КРАСНОЯСРК / RUSSIA", "МВД 24003", "89832068482", null, "ava", 240003, null, new DateTime(2014, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 652893, "ГОР. МИНСК БЕЛАРУСЬ", "Россия, г. Красняосрк, ул. Урванецва, д. 6А, кв. 74", "Россия, г. Красняосрк, ул. Урванецва, д. 6А, кв. 74", 414, "Отделом УФМС РОССИИ ПО КРАСНОЯСРКОМУ КРАЮ В СОВЕТСКОМ Р-НЕ Г.КРАСНОЯСРКА", false, "1111", 111, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 111, "Гор. КРАСНОЯСРК / RUSSIA", "МВД 24003" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactProject_ContactId",
                table: "ContactProject",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactProject_ProjectId",
                table: "ContactProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DocHour_WeekId",
                table: "DocHour",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_EnginerCreaterId",
                table: "Projects",
                column: "EnginerCreaterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCheck_UserProjectId",
                table: "ReportCheck",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProject_ProjectId",
                table: "UserProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProject_UserId",
                table: "UserProject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_FrHourId",
                table: "Week",
                column: "FrHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_MoHourId",
                table: "Week",
                column: "MoHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_SaHourId",
                table: "Week",
                column: "SaHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_SuHourId",
                table: "Week",
                column: "SuHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_ThHourId",
                table: "Week",
                column: "ThHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_TuHourId",
                table: "Week",
                column: "TuHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_UserId",
                table: "Week",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_UserProjectId",
                table: "Week",
                column: "UserProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Week_WeHourId",
                table: "Week",
                column: "WeHourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_FrHourId",
                table: "Week",
                column: "FrHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_MoHourId",
                table: "Week",
                column: "MoHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_SaHourId",
                table: "Week",
                column: "SaHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_SuHourId",
                table: "Week",
                column: "SuHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_ThHourId",
                table: "Week",
                column: "ThHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_TuHourId",
                table: "Week",
                column: "TuHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Week_DocHour_WeHourId",
                table: "Week",
                column: "WeHourId",
                principalTable: "DocHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProject_Projects_ProjectId",
                table: "UserProject");

            migrationBuilder.DropForeignKey(
                name: "FK_DocHour_Week_WeekId",
                table: "DocHour");

            migrationBuilder.DropTable(
                name: "ContactProject");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "ReportCheck");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Week");

            migrationBuilder.DropTable(
                name: "DocHour");

            migrationBuilder.DropTable(
                name: "UserProject");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
