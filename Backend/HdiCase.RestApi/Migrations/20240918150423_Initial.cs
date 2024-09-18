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
                name: "Loggings",
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
                    table.PrimaryKey("PK_Loggings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loggings_AdminLoginData_UserId",
                        column: x => x.UserId,
                        principalTable: "AdminLoginData",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AdminLoginData",
                columns: new[] { "Id", "CreatedDate", "Email", "IsActive", "IsDeveloper", "LastUpdatedDate", "Password", "RoleId" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "superadmin@hdi.com", true, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$3H8U4pSGhsjGgZPRGduFLe79tHJVFhwv.EzimH3FMoycziSuzSaaa", "[1,2,3]" });

            migrationBuilder.CreateIndex(
                name: "IX_Loggings_UserId",
                table: "Loggings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loggings");

            migrationBuilder.DropTable(
                name: "AdminLoginData");
        }
    }
}
