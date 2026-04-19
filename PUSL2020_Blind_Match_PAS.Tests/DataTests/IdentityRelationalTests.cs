using Xunit;
using PUSL2020_Blind_Match_PAS.Models;

namespace PUSL2020_Blind_Match_PAS.Tests.DataTests
{
    public class IdentityRelationalTests
    {
        [Fact]
        public void ProjectProposal_InitializesWithBlindState()
        {
            var proposal = new ProjectProposal();

            Assert.Equal("Pending", proposal.Status);
            Assert.False(proposal.IsIdentityRevealed);
            Assert.Null(proposal.SupervisorId);
        }

        [Fact]
        public void ApplicationUser_SupportsPersonalDataAttribute()
        {
            var property = typeof(ApplicationUser).GetProperty("FullName");
            var hasAttribute = Attribute.IsDefined(property, typeof(Microsoft.AspNetCore.Identity.PersonalDataAttribute));

            Assert.True(hasAttribute);
        }
    }
}
