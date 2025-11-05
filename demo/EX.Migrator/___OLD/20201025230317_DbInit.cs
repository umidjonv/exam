using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EX.Migrator.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(maxLength: 50, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    DurationInMinutes = table.Column<int>(nullable: false),
                    PeriodInDays = table.Column<int>(nullable: false),
                    PassingScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<string>(maxLength: 50, nullable: false),
                    LastActivity = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(maxLength: 50, nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: true),
                    IsBase = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ExamId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    CountOfRegularQuestions = table.Column<int>(nullable: false),
                    CountOfBaseQuestions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamOptions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamOptions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    RegionId = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    LimitOfParticipants = table.Column<int>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInClients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    LoginTime = table.Column<DateTime>(nullable: false),
                    UserSession = table.Column<string>(maxLength: 50, nullable: false),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: false),
                    ComputerDomain = table.Column<string>(maxLength: 50, nullable: true),
                    ComputerName = table.Column<string>(maxLength: 50, nullable: false),
                    ComputerUser = table.Column<string>(maxLength: 50, nullable: false),
                    VideoDriver = table.Column<string>(maxLength: 250, nullable: true),
                    AudioDriver = table.Column<string>(maxLength: 250, nullable: true),
                    MonitorDriver = table.Column<string>(maxLength: 250, nullable: true),
                    IsConnected = table.Column<bool>(nullable: true),
                    ConnectTime = table.Column<DateTime>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    DisconnectTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInClients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    LoginTime = table.Column<DateTime>(nullable: false),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLocales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    Description = table.Column<string>(maxLength: 5000, nullable: true),
                    CultureId = table.Column<string>(nullable: false),
                    QuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLocales_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExamId = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FinishedStatus = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: true),
                    StatusReason = table.Column<string>(nullable: true),
                    StatusDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResults_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamResults_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInExams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    IsPassed = table.Column<bool>(nullable: true),
                    IsAdmit = table.Column<bool>(nullable: false),
                    PassedTime = table.Column<DateTime>(nullable: true),
                    RequestTime = table.Column<DateTime>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInExams_UserInClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "UserInClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInExams_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInExams_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInExams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionInAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 1000, nullable: false),
                    QuestionLocaleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionInAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionInAnswers_QuestionLocales_QuestionLocaleId",
                        column: x => x.QuestionLocaleId,
                        principalTable: "QuestionLocales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResultId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    AnswerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuestionInAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUnits_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUnits_ExamResults_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ExamResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionInCorrects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<int>(nullable: false),
                    AnswerId = table.Column<int>(nullable: false),
                    CultureId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionInCorrects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionInCorrects_QuestionInAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuestionInAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionInCorrects_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUnits_AnswerId",
                table: "AnswerUnits",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUnits_QuestionId",
                table: "AnswerUnits",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerUnits_ResultId",
                table: "AnswerUnits",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamOptions_CategoryId",
                table: "ExamOptions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamOptions_ExamId",
                table: "ExamOptions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ScheduleId",
                table: "ExamResults",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_Code",
                table: "Exams",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionInAnswers_QuestionLocaleId",
                table: "QuestionInAnswers",
                column: "QuestionLocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionInCorrects_AnswerId",
                table: "QuestionInCorrects",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionInCorrects_QuestionId",
                table: "QuestionInCorrects",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLocales_QuestionId",
                table: "QuestionLocales",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ExamId",
                table: "Schedules",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInClients_UserId",
                table: "UserInClients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInClients_UserSession",
                table: "UserInClients",
                column: "UserSession",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInExams_ClientId",
                table: "UserInExams",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInExams_ExamId",
                table: "UserInExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInExams_ScheduleId",
                table: "UserInExams",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInExams_UserId",
                table: "UserInExams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInSessions_UserId",
                table: "UserInSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OwnerId",
                table: "Users",
                column: "OwnerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerUnits");

            migrationBuilder.DropTable(
                name: "ExamOptions");

            migrationBuilder.DropTable(
                name: "QuestionInCorrects");

            migrationBuilder.DropTable(
                name: "UserInExams");

            migrationBuilder.DropTable(
                name: "UserInSessions");

            migrationBuilder.DropTable(
                name: "ExamResults");

            migrationBuilder.DropTable(
                name: "QuestionInAnswers");

            migrationBuilder.DropTable(
                name: "UserInClients");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "QuestionLocales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
