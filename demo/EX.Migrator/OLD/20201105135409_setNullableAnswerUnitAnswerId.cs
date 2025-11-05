using Microsoft.EntityFrameworkCore.Migrations;

namespace EX.Migrator.Migrations
{
    public partial class setNullableAnswerUnitAnswerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                table: "AnswerUnits");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "AnswerUnits",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                table: "AnswerUnits",
                column: "AnswerId",
                principalTable: "QuestionInAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                table: "AnswerUnits");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "AnswerUnits",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                table: "AnswerUnits",
                column: "AnswerId",
                principalTable: "QuestionInAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
