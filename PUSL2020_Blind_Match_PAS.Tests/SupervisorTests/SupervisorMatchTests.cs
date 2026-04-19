using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using System.Security.Claims;

namespace PUSL2020_Blind_Match_PAS.Tests.SupervisorTests
{
    public class SupervisorMatchTests
    {
        [Fact]
        public async Task MyMatches_DisplaysOnlyAssignedAndRevealedProjects()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SupervisorMatchesDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var supervisor = new ApplicationUser { Id = "SUP007", FullName = "Dr. Hiruka" };
                context.Proposals.Add(new ProjectProposal
                {
                    Id = 10,
                    Title = "Matched Idea",
                    Status = "Matched",
                    IsIdentityRevealed = true,
                    SupervisorId = "SUP007"
                });
                await context.SaveChangesAsync();

                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
                userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(supervisor);

                var controller = new SupervisorController(context, userManager.Object);

                var result = await controller.MyMatches();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ProjectProposal>>(viewResult.ViewData.Model);
                Assert.Single(model);
                Assert.True(model.First().IsIdentityRevealed);
            }
        }
    }
}
