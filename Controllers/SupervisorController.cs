using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using System.Threading.Tasks;
using System.Linq;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var anonymousProposals = await _context.Proposals
                .Where(p => p.IsIdentityRevealed == false)
                .ToListAsync();

            return View(anonymousProposals);
        }
    }
}