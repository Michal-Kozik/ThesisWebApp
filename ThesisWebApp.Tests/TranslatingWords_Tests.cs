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
        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        private string GetFileHash(string filename)
        {
            var hash = new SHA1Managed();
            var clearBytes = File.ReadAllBytes(filename);
            var hashedBytes = hash.ComputeHash(clearBytes);
            return ConvertBytesToHex(hashedBytes);
        }

        private string ConvertBytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x"));
            }
            return sb.ToString();
        }



        [Fact]
        public void ValidateWords_FieldIsEmpty_ReturnFalse()
        {
            // Arrange
            var controller = new TranslatingWordsController(GetMockUserManager().Object);
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
        public void ValidateWords_FieldCompleted_ReturnTrue()
        {
            var controller = new TranslatingWordsController(GetMockUserManager().Object);
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
            var controller = new TranslatingWordsController(GetMockUserManager().Object);
            var viewModel = new TranslatingWordsSettingsViewModel()
            {
                NumberOfWords = 3,
                TranslateFromArray = new string[] { "slowo1", "slowo2", "slowo3" },
                TranslateToArray = new string[] { "word1", "word2", "word3" },
            };
            string patternFilePath = "TestFiles/TranslatingWordsTestDir/TranslatingWordsPattern.txt";
            string createdFilePath = "TestFiles/TranslatingWordsTestDir/TranslatingWordsTest.txt";

            controller.SaveExerciseToTxt(viewModel, createdFilePath);
            var originalHash = GetFileHash(patternFilePath);
            var createdHash = GetFileHash(createdFilePath);

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
