using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace PUSL2020_Blind_Match_PAS.Tests.UnitTests
{
    public class LeadDevIntegrationTests
    {
        [Fact]
        public async Task Confirm_ReturnsChallenge_WhenUserIsNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ChallengeTestDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(
                    userStore.Object, null, null, null, null, null, null, null, null);

                userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                           .ReturnsAsync((ApplicationUser)null);

                var controller = new MatchController(context, userManager.Object);

                var result = await controller.Confirm(1);

                Assert.IsType<ChallengeResult>(result);
            }
        }

        [Fact]
        public void Privacy_IsAccessibleByAnonymousUsers()
        {
            var controller = new HomeController();

            var method = typeof(HomeController).GetMethod("Privacy");
            var hasAttribute = method?.GetCustomAttribute<AllowAnonymousAttribute>();

            Assert.NotNull(hasAttribute);
        }
    }
}