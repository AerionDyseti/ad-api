using Microsoft.EntityFrameworkCore.Migrations;

namespace AerionDyseti.Migrations
{
    public partial class userapproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "Approved",
                "AspNetUsers",
                "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Approved",
                "AspNetUsers");
        }
    }
}