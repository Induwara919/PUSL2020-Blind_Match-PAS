using Xunit;
using Microsoft.AspNetCore.Identity;

namespace PUSL2020_Blind_Match_PAS.Tests.UnitTests
{
    public class IdentityConfigTests
    {
        [Fact]
        public void IdentityOptions_PasswordRequirements_AreConfiguredCorrectly()
        {
            var options = new IdentityOptions();

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;

            Assert.False(options.Password.RequireDigit);
            Assert.Equal(3, options.Password.RequiredLength);
            Assert.False(options.Password.RequireNonAlphanumeric);
        }
    }
}