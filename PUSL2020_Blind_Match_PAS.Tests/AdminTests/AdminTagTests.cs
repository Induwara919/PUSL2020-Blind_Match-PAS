using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;

namespace PUSL2020_Blind_Match_PAS.Tests.AdminTests
{
    public class AdminTagTests
    {
        [Fact]
        public async Task ManageTags_AddsNewTagSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TagDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new AdminController(context, null);

                await controller.ManageTags("Artificial Intelligence");

                var tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "Artificial Intelligence");
                Assert.NotNull(tag);
                Assert.Equal("Artificial Intelligence", tag.Name);
            }
        }
    }
}
