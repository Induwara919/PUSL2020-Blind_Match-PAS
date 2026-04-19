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
using Microsoft.AspNetCore.Http;

namespace PUSL2020_Blind_Match_PAS.Tests.UnitTests
{
    public class MatchControllerTests
    {
        [Fact]
        public async Task ConfirmMatch_UpdatesStatusAndRevealsIdentity()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MatchTestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var testProposal = new ProjectProposal
                {
                    Id = 1,
                    Title = "AI Research",
                    Status = "Pending",
                    IsIdentityRevealed = false
                };
                context.Proposals.Add(testProposal);
                await context.SaveChangesAsync();

                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

                var supervisor = new ApplicationUser { Id = "S1", FullName = "Dr. Kamal", Email = "kamal@nsbm.ac.lk" };
                userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(supervisor);

                var controller = new MatchController(context, userManager.Object);

                var result = await controller.Confirm(1);

                var updatedProposal = await context.Proposals.FindAsync(1);
                Assert.Equal("Matched", updatedProposal.Status);
                Assert.True(updatedProposal.IsIdentityRevealed);
                Assert.Equal("Dr. Kamal", updatedProposal.SupervisorName);
            }
        }
    }
}