using Microsoft.EntityFrameworkCore.Migrations;

namespace ThesisWebApp.Data.Migrations
{
    public partial class updateDatabase_ver_1_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfExercises",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "NumberOfTests",
                table: "Statistics");

            migrationBuilder.AddColumn<int>(
                name: "ExercisesA1",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesA2",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesB1",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesB2",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesC1",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesC2",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesUknown",
                table: "Statistics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExercisesA1",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesA2",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesB1",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesB2",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesC1",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesC2",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ExercisesUknown",
                table: "Statistics");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfExercises",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTests",
                table: "Statistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
