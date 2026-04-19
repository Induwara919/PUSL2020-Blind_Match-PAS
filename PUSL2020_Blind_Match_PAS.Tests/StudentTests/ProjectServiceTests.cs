using Xunit;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Services;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;

namespace PUSL2020_Blind_Match_PAS.Tests.StudentTests
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task ConfirmMatchAsync_TriggersIdentityReveal()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ServiceTestDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var proposal = new ProjectProposal { Id = 5, Title = "IoT", Status = "Pending", IsIdentityRevealed = false };
                context.Proposals.Add(proposal);
                await context.SaveChangesAsync();

                var service = new ProjectService(context);
                var supervisor = new ApplicationUser { Id = "SUP01", FullName = "Dr. Silva", Email = "silva@nsbm.ac.lk" };

                var result = await service.ConfirmMatchAsync(5, supervisor);

                var updated = await context.Proposals.FindAsync(5);
                Assert.True(result);
                Assert.Equal("Matched", updated.Status);
                Assert.True(updated.IsIdentityRevealed);
                Assert.Equal("Dr. Silva", updated.SupervisorName);
            }
        }
    }
}
