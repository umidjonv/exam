using Microsoft.EntityFrameworkCore.Migrations;

namespace EX.Migrator.Migrations
{
    public partial class Add_Exam_CultureId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CultureId",
                table: "Exams",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CultureId",
                table: "Exams");
        }
    }
}
