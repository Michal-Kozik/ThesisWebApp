using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ThesisWebApp.Controllers;
using ThesisWebApp.ViewModels;
using Xunit;

namespace ThesisWebApp.Tests
{
    public class ReadingTitles_Tests
    {
        [Fact]
        public void ValidateInputs_FieldIsEmpty_ReturnFalse()
        {
            // Arrange
            var mockUserManager = new MockUserManager();
            var controller = new ReadingTitlesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new ReadingTitlesSettingsViewModel()
            {
                NumberOfParagraphs = 3,
                Paragraphs = new string[] { "paragraph1", "", "paragraph3" },
                CorrectTitles = new string[] { "title1", "title2", "" },
                NumberOfAdditionalTitles = 2,
                AdditionalTitles = new string[] { "additional1", "" }
            };

            // Act
            bool result = controller.ValidateInputs(viewModel);

            // Assert
            Assert.False(result, "Input has empty fields");
        }

        [Fact]
        public void ValidateWords_FieldCompleted_ReturnTrue()
        {
            var mockUserManager = new MockUserManager();
            var controller = new ReadingTitlesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new ReadingTitlesSettingsViewModel()
            {
                NumberOfParagraphs = 3,
                Paragraphs = new string[] { "paragraph1", "paragraph2", "paragraph3" },
                CorrectTitles = new string[] { "title1", "title2", "title3" },
                NumberOfAdditionalTitles = 2,
                AdditionalTitles = new string[] { "additional1", "additional2" }
            };

            bool result = controller.ValidateInputs(viewModel);

            Assert.True(result, "Inputs are correct");
        }

        [Fact]
        public void SaveExerciseToTxt_SaveSuccess()
        {
            var mockUserManager = new MockUserManager();
            var controller = new ReadingTitlesController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new ReadingTitlesSettingsViewModel()
            {
                NumberOfParagraphs = 3,
                Paragraphs = new string[] { "paragraph1", "paragraph2", "paragraph3" },
                CorrectTitles = new string[] { "title1", "title2", "title3" },
                NumberOfAdditionalTitles = 2,
                AdditionalTitles = new string[] { "additional1", "additional2" }
            };
            var fileHash = new FileHash();
            string patternFilePath = "TestFiles/ReadingTitlesTestDir/ReadingTitlesPattern.txt";
            string createdFilePath = "TestFiles/ReadingTitlesTestDir/ReadingTitlesTest.txt";

            controller.SaveExerciseToTxt(viewModel, createdFilePath);
            var originalHash = fileHash.GetFileHash(patternFilePath);
            var createdHash = fileHash.GetFileHash(createdFilePath);

            Assert.Equal(originalHash, createdHash);
        }

        [Fact]
        public void ReadExerciseFromTxt_ReadSuccess()
        {
            var viewModel = new ReadingTitlesSettingsViewModel()
            {
                NumberOfParagraphs = 3,
                Paragraphs = new string[] { "paragraph1", "paragraph2", "paragraph3", null, null, null },
                CorrectTitles = new string[] { "title1", "title2", "title3", null, null, null },
                NumberOfAdditionalTitles = 2,
                AdditionalTitles = new string[] { "additional1", "additional2", null, null, null, null }
            };
            string patternFilePath = "TestFiles/ReadingTitlesTestDir/ReadingTitlesPattern.txt";

            ReadingTitlesSettingsViewModel result = ReadingTitlesController.ReadExerciseFromTxt(patternFilePath);
            var resultObject = JsonConvert.SerializeObject(result);
            var patternObject = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(patternObject, resultObject);
        }
    }
}
