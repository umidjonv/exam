using Microsoft.EntityFrameworkCore.Migrations;

namespace EX.Migrator.Migrations
{
    public partial class questionIDForResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "QuestionIds",
                table: "ExamResults",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionIds",
                table: "ExamResults");

            
        }
    }
}
