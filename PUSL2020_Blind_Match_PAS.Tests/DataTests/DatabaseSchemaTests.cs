using Xunit;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;

namespace PUSL2020_Blind_Match_PAS.Tests.DataTests
{
    public class DatabaseSchemaTests
    {
        [Fact]
        public async Task Database_EnforcesUniqueConstraint_OnTagNames()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UniqueTagDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Tags.Add(new Tag { Name = "Web Development" });
                await context.SaveChangesAsync();

                var model = context.Model.FindEntityType(typeof(Tag));
                var index = model.GetIndexes();

                Assert.Contains(index, i => i.Properties.Any(p => p.Name == "Name") && i.IsUnique);
            }
        }
    }
}
