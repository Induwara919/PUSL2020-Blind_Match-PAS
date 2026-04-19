using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using System.Collections.Generic;
using System.Linq;

namespace PUSL2020_Blind_Match_PAS.Tests.SupervisorTests
{
    public class SupervisorDashboardTests
    {
        [Fact]
        public async Task Dashboard_OnlyShowsAnonymousAndPendingProposals()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BlindReviewDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Proposals.AddRange(new List<ProjectProposal> {
                    new ProjectProposal { Id = 1, Title = "Hidden App", IsIdentityRevealed = false, Status = "Pending" },
                    new ProjectProposal { Id = 2, Title = "Revealed App", IsIdentityRevealed = true, Status = "Matched" }
                });
                await context.SaveChangesAsync();

                var controller = new SupervisorController(context, null);

                var result = await controller.Dashboard(null);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ProjectProposal>>(viewResult.ViewData.Model);
                Assert.Single(model);
                Assert.Equal("Hidden App", model.First().Title);
            }
        }

        [Fact]
        public async Task Dashboard_FiltersByResearchAreaCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FilterDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Proposals.AddRange(new List<ProjectProposal> {
                    new ProjectProposal { Id = 3, Title = "AI Project", ResearchArea = "AI", Status = "Pending" },
                    new ProjectProposal { Id = 4, Title = "Web Project", ResearchArea = "Web", Status = "Pending" }
                });
                await context.SaveChangesAsync();

                var controller = new SupervisorController(context, null);

                var result = await controller.Dashboard("AI");

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ProjectProposal>>(viewResult.ViewData.Model);
                Assert.All(model, p => Assert.Equal("AI", p.ResearchArea));
            }
        }

        [Fact]
        public async Task Dashboard_ReturnsEmptyList_WhenNoProposalsMatchFilter()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyFilterDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Proposals.Add(new ProjectProposal { Id = 100, Title = "SafeNet", ResearchArea = "Cybersecurity", Status = "Pending" });
                await context.SaveChangesAsync();

                var controller = new SupervisorController(context, null);

                var result = await controller.Dashboard("Blockchain");

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ProjectProposal>>(viewResult.ViewData.Model);
                Assert.Empty(model);
            }
        }
    }
}
