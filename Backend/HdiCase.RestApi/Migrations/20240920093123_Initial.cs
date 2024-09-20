using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HdiCase.RestApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminLoginData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeveloper = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLoginData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiIsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApiPerMinuteMaximumRequestCount = table.Column<int>(type: "int", nullable: false),
                    AggrementResultWebhookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HighestGood = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionKeys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableId = table.Column<int>(type: "int", nullable: true),
                    OperationType = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logging_AdminLoginData_UserId",
                        column: x => x.UserId,
                        principalTable: "AdminLoginData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Aggrement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RiskRate = table.Column<int>(type: "int", nullable: false),
                    RiskAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aggrement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aggrement_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AggrementAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggrementId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RelatedObjectives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Efforts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Costs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Risks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggrementAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggrementAnalysis_AdminLoginData_UserId",
                        column: x => x.UserId,
                        principalTable: "AdminLoginData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AggrementAnalysis_Aggrement_AggrementId",
                        column: x => x.AggrementId,
                        principalTable: "Aggrement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AggrementContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggrementId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggrementContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggrementContact_Aggrement_AggrementId",
                        column: x => x.AggrementId,
                        principalTable: "Aggrement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AggrementFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AggrementId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggrementFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AggrementFile_Aggrement_AggrementId",
                        column: x => x.AggrementId,
                        principalTable: "Aggrement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminLoginData",
                columns: new[] { "Id", "CreatedDate", "Email", "IsActive", "IsDeveloper", "LastUpdatedDate", "Password", "RoleId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@hdi.com", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$kZc6wJW/Zqahnn4zN8ZiDuK4tzVO7Q8GdCqOrerGdkUhgb7kys5lK", "[1,2,3]" });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "AggrementResultWebhookUrl", "ApiIsActive", "ApiKey", "ApiPerMinuteMaximumRequestCount", "CreatedDate", "Emails", "LastUpdatedDate", "Name", "Phones" },
                values: new object[] { 1, "http://kadirguloglu.com/aggrementResult", true, "b9bafd21c7f740df827edd91281de753ebe9eb66ec0847b4954ac5ddccfd37fa", 100, new DateTime(2024, 9, 20, 12, 31, 23, 292, DateTimeKind.Local).AddTicks(9330), "[\"kadirguloglu1@gmail.com\"]", new DateTime(2024, 9, 20, 12, 31, 23, 292, DateTimeKind.Local).AddTicks(9380), "Test company", "[\"05074983810\"]" });

            migrationBuilder.CreateIndex(
                name: "IX_Aggrement_CompanyId",
                table: "Aggrement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AggrementAnalysis_AggrementId",
                table: "AggrementAnalysis",
                column: "AggrementId");

            migrationBuilder.CreateIndex(
                name: "IX_AggrementAnalysis_UserId",
                table: "AggrementAnalysis",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AggrementContact_AggrementId",
                table: "AggrementContact",
                column: "AggrementId");

            migrationBuilder.CreateIndex(
                name: "IX_AggrementFile_AggrementId",
                table: "AggrementFile",
                column: "AggrementId");

            migrationBuilder.CreateIndex(
                name: "IX_Logging_UserId",
                table: "Logging",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggrementAnalysis");

            migrationBuilder.DropTable(
                name: "AggrementContact");

            migrationBuilder.DropTable(
                name: "AggrementFile");

            migrationBuilder.DropTable(
                name: "Logging");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Aggrement");

            migrationBuilder.DropTable(
                name: "AdminLoginData");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
