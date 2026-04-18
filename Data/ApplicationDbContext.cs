using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Models;

namespace PUSL2020_Blind_Match_PAS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectProposal> Proposals { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}