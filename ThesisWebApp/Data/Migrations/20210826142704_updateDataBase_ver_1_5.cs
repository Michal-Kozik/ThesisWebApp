using Microsoft.EntityFrameworkCore.Migrations;

namespace ThesisWebApp.Data.Migrations
{
    /* Baza posiada nowa tabele Statistics 1 do 1 z Application User,
       dodatkowo dodano kilka nowych pol do istniejacyh tabel. */
    public partial class updateDataBase_ver_1_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseExams");

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Exercises",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Exams",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExercisesPattern",
                table: "Exams",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Exams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    StatisticsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfTests = table.Column<int>(nullable: false),
                    NumberOfExercises = table.Column<int>(nullable: false),
                    ApplicationUserID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.StatisticsID);
                    table.ForeignKey(
                        name: "FK_Statistics_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserID",
                table: "Exams",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_ApplicationUserID",
                table: "Statistics",
                column: "ApplicationUserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserID",
                table: "Exams",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserID",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ApplicationUserID",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExercisesPattern",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Exams");

            migrationBuilder.CreateTable(
                name: "ExerciseExams",
                columns: table => new
                {
                    ExerciseExamsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExams", x => x.ExerciseExamsID);
                    table.ForeignKey(
                        name: "FK_ExerciseExams_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseExams_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExamID",
                table: "ExerciseExams",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExerciseID",
                table: "ExerciseExams",
                column: "ExerciseID");
        }
    }
}
