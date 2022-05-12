using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    public partial class AddedDefaultPermissionGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionGroups",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[] { new Guid("9cc607c1-7b93-4245-98f5-0d788cf94895"), "Everyone", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionGroups",
                keyColumn: "Id",
                keyValue: new Guid("9cc607c1-7b93-4245-98f5-0d788cf94895"));
        }
    }
}
