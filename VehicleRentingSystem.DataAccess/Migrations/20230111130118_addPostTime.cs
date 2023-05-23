using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentingSystem.DataAccess.Migrations
{
    public partial class addPostTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PostTime",
                table: "Post_Cars",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TargetTime",
                table: "Post_Cars",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTime",
                table: "Post_Cars");

            migrationBuilder.DropColumn(
                name: "TargetTime",
                table: "Post_Cars");
        }
    }
}
