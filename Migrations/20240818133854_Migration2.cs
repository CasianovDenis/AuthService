using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Myproject.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenExpire",
                schema: "auth",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                schema: "auth",
                table: "Users",
                newName: "SecurityStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecurityStamp",
                schema: "auth",
                table: "Users",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpire",
                schema: "auth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
