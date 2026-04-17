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

        public async Task<IActionResult> Dashboard(string searchArea)
        {
            var query = _context.Proposals.Where(p => !p.IsIdentityRevealed);
            if (!string.IsNullOrEmpty(searchArea))
            {
                query = query.Where(p => p.ResearchArea == searchArea);
            }
            var proposals = await query.ToListAsync();
            ViewBag.CurrentFilter = searchArea;
            return View(proposals);
        }

        public async Task<IActionResult> MyMatches()
        {
  
            var matchedProjects = await _context.Proposals
                .Where(p => p.IsIdentityRevealed == true && p.Status == "Matched")
                .ToListAsync();

            return View(matchedProjects);
        }
    }
}
