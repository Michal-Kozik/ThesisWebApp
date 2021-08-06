using Microsoft.EntityFrameworkCore.Migrations;

namespace ThesisWebApp.Data.Migrations
{
    // Dodano relacje miedzy 'Exercises' a 'Users'.
    public partial class updateDataBase_ver_1_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Exams_ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExamForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExamID",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserForeignKey",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Exercises",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfExercise",
                table: "Exercises",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ApplicationUserForeignKey",
                table: "Exercises",
                column: "ApplicationUserForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserForeignKey",
                table: "Exercises",
                column: "ApplicationUserForeignKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ApplicationUserForeignKey",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TypeOfExercise",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "ExamForeignKey",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamID",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExamForeignKey",
                table: "Exercises",
                column: "ExamForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Exams_ExamForeignKey",
                table: "Exercises",
                column: "ExamForeignKey",
                principalTable: "Exams",
                principalColumn: "ExamID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
