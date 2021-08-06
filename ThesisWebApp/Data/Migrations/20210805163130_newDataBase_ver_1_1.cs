using Microsoft.EntityFrameworkCore.Migrations;

namespace ThesisWebApp.Data.Migrations
{
    public partial class newDataBase_ver_1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserID",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Exams_ExamID",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TypeOfExercises_TypeOfExerciseID",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "TypeOfExercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExamID",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_TypeOfExerciseID",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ApplicationUserID",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "TypeOfExerciseID",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ExamForeignKey",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "Exams",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserForeignKey",
                table: "Exams",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseExams",
                columns: table => new
                {
                    ExerciseExamsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamForeignKey = table.Column<int>(nullable: true),
                    ExamID = table.Column<int>(nullable: false),
                    ExerciseForeignKey = table.Column<int>(nullable: true),
                    ExerciseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExams", x => x.ExerciseExamsID);
                    table.ForeignKey(
                        name: "FK_ExerciseExams_Exams_ExamForeignKey",
                        column: x => x.ExamForeignKey,
                        principalTable: "Exams",
                        principalColumn: "ExamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseExams_Exercises_ExerciseForeignKey",
                        column: x => x.ExerciseForeignKey,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExamForeignKey",
                table: "Exercises",
                column: "ExamForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserForeignKey",
                table: "Exams",
                column: "ApplicationUserForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExamForeignKey",
                table: "ExerciseExams",
                column: "ExamForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExerciseForeignKey",
                table: "ExerciseExams",
                column: "ExerciseForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserForeignKey",
                table: "Exams",
                column: "ApplicationUserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Exams_ExamForeignKey",
                table: "Exercises",
                column: "ExamForeignKey",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Exams_ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "ExerciseExams");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "TypeOfExerciseID",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TypeOfExercises",
                columns: table => new
                {
                    TypeOfExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfExercises", x => x.TypeOfExerciseID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExamID",
                table: "Exercises",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TypeOfExerciseID",
                table: "Exercises",
                column: "TypeOfExerciseID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserID",
                table: "Exams",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserID",
                table: "Exams",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Exams_ExamID",
                table: "Exercises",
                column: "ExamID",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TypeOfExercises_TypeOfExerciseID",
                table: "Exercises",
                column: "TypeOfExerciseID",
                principalTable: "TypeOfExercises",
                principalColumn: "TypeOfExerciseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
