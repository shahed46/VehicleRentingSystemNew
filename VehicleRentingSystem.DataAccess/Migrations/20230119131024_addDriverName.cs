using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentingSystem.DataAccess.Migrations
{
    public partial class addDriverName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Complain",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Complain");
        }
    }
}
