using Microsoft.EntityFrameworkCore.Migrations;

namespace EX.Migrator.Migrations
{
    public partial class UserInSessionRemoveDisplayDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonitorDriver",
                table: "UserInClients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MonitorDriver",
                table: "UserInClients",
                type: "varchar(250) CHARACTER SET utf8mb4",
                maxLength: 250,
                nullable: true);
        }
    }
}
