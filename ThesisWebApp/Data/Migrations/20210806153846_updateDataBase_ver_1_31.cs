using Microsoft.EntityFrameworkCore.Migrations;

namespace ThesisWebApp.Data.Migrations
{
    public partial class updateDataBase_ver_1_31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExams_Exams_ExamForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExams_Exercises_ExerciseForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExams_ExamForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExams_ExerciseForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExamForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropColumn(
                name: "ExerciseForeignKey",
                table: "ExerciseExams");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Exams");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ApplicationUserID",
                table: "Exercises",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExamID",
                table: "ExerciseExams",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExerciseID",
                table: "ExerciseExams",
                column: "ExerciseID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExams_Exams_ExamID",
                table: "ExerciseExams",
                column: "ExamID",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExams_Exercises_ExerciseID",
                table: "ExerciseExams",
                column: "ExerciseID",
                principalTable: "Exercises",
                principalColumn: "ExerciseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserID",
                table: "Exercises",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExams_Exams_ExamID",
                table: "ExerciseExams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseExams_Exercises_ExerciseID",
                table: "ExerciseExams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserID",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ApplicationUserID",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExams_ExamID",
                table: "ExerciseExams");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseExams_ExerciseID",
                table: "ExerciseExams");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserForeignKey",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamForeignKey",
                table: "ExerciseExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseForeignKey",
                table: "ExerciseExams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserForeignKey",
                table: "Exams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ApplicationUserForeignKey",
                table: "Exercises",
                column: "ApplicationUserForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExamForeignKey",
                table: "ExerciseExams",
                column: "ExamForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseExams_ExerciseForeignKey",
                table: "ExerciseExams",
                column: "ExerciseForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserForeignKey",
                table: "Exams",
                column: "ApplicationUserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_ApplicationUserForeignKey",
                table: "Exams",
                column: "ApplicationUserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExams_Exams_ExamForeignKey",
                table: "ExerciseExams",
                column: "ExamForeignKey",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseExams_Exercises_ExerciseForeignKey",
                table: "ExerciseExams",
                column: "ExerciseForeignKey",
                principalTable: "Exercises",
                principalColumn: "ExerciseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserForeignKey",
                table: "Exercises",
                column: "ApplicationUserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
