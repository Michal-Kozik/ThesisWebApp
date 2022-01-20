using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ThesisWebApp.Controllers;
using ThesisWebApp.Models;
using ThesisWebApp.ViewModels;
using Xunit;
using Newtonsoft.Json;

namespace ThesisWebApp.Tests
{
    public class TranslatingWords_Tests
    {
        [Fact]
        public void ValidateWords_FieldIsEmpty_ReturnFalse()
        {
            // Arrange
            var mockUserManager = new MockUserManager();
            var controller = new TranslatingWordsController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateFromArray = new string[] { "word1", "", "word3" },
                TranslateToArray = new string[] { "slowo1", "", "slowo3" },
            };

            // Act
            bool result = controller.ValidateWords(viewModel);

            // Assert
            Assert.False(result, "Input has empty fields");
        }

        [Fact]
        public void ValidateWords_FieldsCompleted_ReturnTrue()
        {
            var mockUserManager = new MockUserManager();
            var controller = new TranslatingWordsController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateFromArray = new string[] { "word1", "word2", "word3" },
                TranslateToArray = new string[] { "slowo1", "slowo2", "slowo3" },
            };

            bool result = controller.ValidateWords(viewModel);

            Assert.True(result, "Inputs are correct");
        }

        [Fact]
        public void SaveExerciseToTxt_SaveSuccess()
        {
            var mockUserManager = new MockUserManager();
            var controller = new TranslatingWordsController(mockUserManager.GetMockUserManager().Object);
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateFromArray = new string[] { "slowo1", "slowo2", "slowo3" },
                TranslateToArray = new string[] { "word1", "word2", "word3" },
            };
            var fileHash = new FileHash();
            string patternFilePath = "TestFiles/TranslatingWordsTestDir/TranslatingWordsPattern.txt";
            string createdFilePath = "TestFiles/TranslatingWordsTestDir/TranslatingWordsTest.txt";

            controller.SaveExerciseToTxt(viewModel, createdFilePath);
            var originalHash = fileHash.GetFileHash(patternFilePath);
            var createdHash = fileHash.GetFileHash(createdFilePath);

            Assert.Equal(originalHash, createdHash);
        }

        [Fact]
        public void ReadExerciseFromTxt_ReadSuccess()
        {
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateFromArray = new string[] { "slowo1", "slowo2", "slowo3", null, null, null, null, null, null, null, null, null, null, null, null },
                TranslateToArray = new string[] { "word1", "word2", "word3", null, null, null, null, null, null, null, null, null, null, null, null }
            };
            string patternFilePath = "TestFiles/TranslatingWordsTestDir/TranslatingWordsPattern.txt";

            TranslatingWordsSettingsViewModel result = TranslatingWordsController.ReadExerciseFromTxt(patternFilePath);
            var resultObject = JsonConvert.SerializeObject(result);
            var patternObject = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(patternObject, resultObject);
        }
    }
}
