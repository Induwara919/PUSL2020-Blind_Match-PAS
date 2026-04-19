using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;

namespace PUSL2020_Blind_Match_PAS.Tests.AdminTests
{
    public class AdminOversightTests
    {
        [Fact]
        public async Task ResetMatch_ReturnsProposalToPendingState()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OversightDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var matchedProposal = new ProjectProposal
                {
                    Id = 50,
                    Status = "Matched",
                    IsIdentityRevealed = true,
                    SupervisorName = "Dr. Anton"
                };
                context.Proposals.Add(matchedProposal);
                await context.SaveChangesAsync();

                var controller = new AdminController(context, null);

                await controller.ResetMatch(50);

                var resetProposal = await context.Proposals.FindAsync(50);
                Assert.Equal("Pending", resetProposal.Status);
                Assert.False(resetProposal.IsIdentityRevealed);
                Assert.Null(resetProposal.SupervisorName);
            }
        }
    }
}
