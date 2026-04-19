using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PUSL2020_Blind_Match_PAS.Tests.StudentTests
{
    public class ProposalControllerTests
    {
        [Fact]
        public async Task Create_SetsDefaultStatusToPendingAndHidden()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentCreateDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

                var student = new ApplicationUser { Id = "STU01", FullName = "Isumi Perera", StudentId = "S123" };
                userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(student);

                var controller = new ProposalController(context, userManager.Object);
                var newProposal = new ProjectProposal { Title = "Blockchain Voting", Abstract = "Research paper..." };

                await controller.Create(newProposal);

                var saved = await context.Proposals.FirstAsync();
                Assert.Equal("Pending", saved.Status);
                Assert.False(saved.IsIdentityRevealed);
                Assert.Equal("Isumi Perera", saved.StudentName);
            }
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_IfProposalIsAlreadyMatched()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentEditDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var matchedProposal = new ProjectProposal { Id = 99, Title = "AI", Status = "Matched" };
                context.Proposals.Add(matchedProposal);
                await context.SaveChangesAsync();

                var controller = new ProposalController(context, null);

                var result = await controller.Edit(99);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Cannot edit a proposal that has already been matched.", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Withdraw_DoesNotDelete_IfStatusIsMatched()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StudentWithdrawDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var matchedProposal = new ProjectProposal { Id = 10, Title = "AI Study", Status = "Matched" };
                context.Proposals.Add(matchedProposal);
                await context.SaveChangesAsync();

                var controller = new ProposalController(context, null);

                await controller.Withdraw(10);

                var exists = await context.Proposals.AnyAsync(p => p.Id == 10);
                Assert.True(exists);
            }
        }
    }
}
