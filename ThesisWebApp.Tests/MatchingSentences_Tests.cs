using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ThesisWebApp.Controllers;
using ThesisWebApp.ViewModels;
using Xunit;

namespace ThesisWebApp.Tests
{
    public class MatchingSentences_Tests
    {
        [Fact]
        public void ValidateInputs_FieldIsEmpty_ReturnFalse()
        {
            // Arrange
            var mockUserManager = new MockUserManager();
            var controller = new MatchingSentencesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new MatchingSentencesSettingsViewModel()
            {
                NumberOfSentences = 3,
                SentencesFirstPart = new string[] { "sen1", "", "sen3" },
                SentencesSecondPart = new string[] { "tence1", "tence2", "" }
            };

            // Act
            bool result = controller.ValidateInputs(viewModel);

            // Assert
            Assert.False(result, "Input has empty fields");
        }

        [Fact]
        public void ValidateInputs_FieldsCompleted_ReturnTrue()
        {
            var mockUserManager = new MockUserManager();
            var controller = new MatchingSentencesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new MatchingSentencesSettingsViewModel()
            {
                NumberOfSentences = 3,
                SentencesFirstPart = new string[] { "sen1", "sen2", "sen3" },
                SentencesSecondPart = new string[] { "tence1", "tence2", "tence3" }
            };

            bool result = controller.ValidateInputs(viewModel);

            Assert.True(result, "Inputs are correct");
        }

        [Fact]
        public void SaveExerciseToTxt_SaveSuccess()
        {
            var mockUserManager = new MockUserManager();
            var controller = new MatchingSentencesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new MatchingSentencesSettingsViewModel()
            {
                NumberOfSentences = 3,
                SentencesFirstPart = new string[] { "sen1", "sen2", "sen3" },
                SentencesSecondPart = new string[] { "tence1", "tence2", "tence3" }
            };
            var fileHash = new FileHash();
            string patternFilePath = "TestFiles/MatchingSentencesTestDir/MatchingSentencesPattern.txt";
            string createdFilePath = "TestFiles/MatchingSentencesTestDir/MatchingSentencesTest.txt";

            controller.SaveExerciseToTxt(viewModel, createdFilePath);
            var originalHash = fileHash.GetFileHash(patternFilePath);
            var createdHash = fileHash.GetFileHash(createdFilePath);

            Assert.Equal(originalHash, createdHash);
        }

        [Fact]
        public void ReadExerciseFromTxt_ReadSuccess()
        {
            var viewModel = new MatchingSentencesSettingsViewModel()
            {
                NumberOfSentences = 3,
                SentencesFirstPart = new string[] { "sen1", "sen2", "sen3", null, null, null, null, null, null, null },
                SentencesSecondPart = new string[] { "tence1", "tence2", "tence3", null, null, null, null, null, null, null }
            };
            string patternFilePath = "TestFiles/MatchingSentencesTestDir/MatchingSentencesPattern.txt";

            MatchingSentencesSettingsViewModel result = MatchingSentencesController.ReadExerciseFromTxt(patternFilePath);
            var resultObject = JsonConvert.SerializeObject(result);
            var patternObject = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(patternObject, resultObject);
        }
    }
}
