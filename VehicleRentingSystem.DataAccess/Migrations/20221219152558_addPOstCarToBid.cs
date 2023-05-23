using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentingSystem.DataAccess.Migrations
{
    public partial class addPOstCarToBid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Bids",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_PostId",
                table: "Bids",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Post_Cars_PostId",
                table: "Bids",
                column: "PostId",
                principalTable: "Post_Cars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Post_Cars_PostId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_PostId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
