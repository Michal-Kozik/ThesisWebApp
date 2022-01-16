using System;
using Xunit;
using ThesisWebApp.Models;

namespace ThesisWebApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var exercise = new Exercise { Name = "Testowa nazwa" };

            // Act
            exercise.Name = "Nowa nazwa";

            // Assert
            Assert.Equal("Nowa nazwa", exercise.Name);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            var exercise = new Exercise { LevelOfExercise = ExerciseLevel.B1 };

            // Act
            exercise.LevelOfExercise = ExerciseLevel.B2;

            // Assert
            Assert.Equal(ExerciseLevel.B1, exercise.LevelOfExercise);
        }
    }
}
