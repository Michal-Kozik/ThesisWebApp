using System;
using System.Collections.Generic;
using System.Text;
using ThesisWebApp.Controllers;
using ThesisWebApp.ViewModels;
using Xunit;

namespace ThesisWebApp.Tests
{
    public class Exercise_Tests
    {
        [Fact]
        public void TranslatingWordsCheck_CorrectOutput()
        {
            // Arrange
            var mockUserManager = new MockUserManager();
            var controller = new ExerciseController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateToArray = new string[] { "answer1", "answer2", "answer3" },
                UserAnswers = new string[] { "", "answer2", "smth"}
            };

            // Act
            int result = controller.TranslatingWordsCheck(viewModel);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ReadingTitlesCheck_CorrectOutput()
        {
            var mockUserManager = new MockUserManager();
            var controller = new ExerciseController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new ReadingTitlesSettingsViewModel()
            {
                NumberOfParagraphs = 3,
                CorrectTitles = new string[] { "answer1", "answer2", "answer3" },
                UserAnswers = new string[] { "", "answer2", "smth" }
            };

            int result = controller.ReadingTitlesCheck(viewModel);

            Assert.Equal(1, result);
        }

        [Fact]
        public void MatchingSentencesCheck_CorrectOutput()
        {
            var mockUserManager = new MockUserManager();
            var controller = new ExerciseController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new MatchingSentencesSettingsViewModel()
            {
                NumberOfSentences = 3,
                SentencesSecondPart = new string[] { "answer1", "answer2", "answer3" },
                UserAnswers = new string[] { "", "answer2", "smth" }
            };

            int result = controller.MatchingSentencesCheck(viewModel);

            Assert.Equal(1, result);
        }
    }
}
