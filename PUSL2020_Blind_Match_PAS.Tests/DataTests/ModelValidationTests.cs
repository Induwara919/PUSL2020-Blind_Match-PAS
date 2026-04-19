using Xunit;
using System.ComponentModel.DataAnnotations;
using PUSL2020_Blind_Match_PAS.Models;
using System.Collections.Generic;

namespace PUSL2020_Blind_Match_PAS.Tests.DataTests
{
    public class ModelValidationTests
    {
        [Theory]
        [InlineData("12345678", true)] 
        [InlineData("12345", false)]    
        [InlineData("ABC12345", false)] 
        public void StudentId_Regex_ValidatesCorrectly(string studentId, bool expectedValid)
        {
            var user = new ApplicationUser { FullName = "Test User", StudentId = studentId };
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, true);

            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData("Artificial Intelligence", true)]
        [InlineData("Cyber123", false)] 
        public void TagName_Regex_RestrictsNonLetters(string tagName, bool expectedValid)
        {
            var tag = new Tag { Name = tagName };
            var context = new ValidationContext(tag);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(tag, context, results, true);

            Assert.Equal(expectedValid, isValid);
        }

        [Fact]
        public void ProjectProposal_Fails_WhenAbstractExceedsLimit()
        {
            var longAbstract = new string('A', 2001);
            var proposal = new ProjectProposal
            {
                Title = "Valid Title",
                Abstract = longAbstract,
                TechnicalStack = "C#",
                ResearchArea = "AI"
            };

            var context = new ValidationContext(proposal);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(proposal, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Abstract cannot exceed 2000 characters.");
        }
    }
}
