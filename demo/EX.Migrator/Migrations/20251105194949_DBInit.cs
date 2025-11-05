using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EX.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class DBInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    DurationInMinutes = table.Column<int>(type: "integer", nullable: false),
                    PeriodInDays = table.Column<int>(type: "integer", nullable: false),
                    PassingScore = table.Column<int>(type: "integer", nullable: false),
                    CultureId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    IsBase = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CountOfRegularQuestions = table.Column<int>(type: "integer", nullable: false),
                    CountOfBaseQuestions = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    ExamId = table.Column<int>(type: "integer", nullable: false),
                    LimitOfParticipants = table.Column<int>(type: "integer", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ModifiedIp = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LoginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserSession = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ComputerDomain = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ComputerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ComputerUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VideoDriver = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AudioDriver = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    IsConnected = table.Column<bool>(type: "boolean", nullable: true),
                    ConnectTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    DisconnectTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LoginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    CultureId = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLocales_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: true),
                    StatusReason = table.Column<string>(type: "text", nullable: true),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    QuestionIds = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    ExamId = table.Column<int>(type: "integer", nullable: false),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: true),
                    IsAdmit = table.Column<bool>(type: "boolean", nullable: false),
                    PassedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequestTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInExams", x => x.Id);
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
                        name: "FK_UserInExams_UserInClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "UserInClients",
                        principalColumn: "Id");
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    QuestionLocaleId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<char>(type: "character(1)", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResultId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AnswerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerUnits_ExamResults_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ExamResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerUnits_QuestionInAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuestionInAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnswerUnits_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionInCorrects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    CultureId = table.Column<string>(type: "text", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerUnits");

            migrationBuilder.DropTable(
                name: "Documents");

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
