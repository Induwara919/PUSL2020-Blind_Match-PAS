using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;
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

        public async Task<IActionResult> Dashboard(string selectedArea)
        {
            ViewBag.AvailableTags = await _context.Tags.Select(t => t.Name).ToListAsync();
            
            var query = _context.Proposals.Where(p => !p.IsIdentityRevealed && p.Status != "Matched");

            if (!string.IsNullOrEmpty(selectedArea))
            {
                query = query.Where(p => p.ResearchArea == selectedArea);
                ViewBag.SelectedArea = selectedArea;
            }

            var proposals = await query.ToListAsync();
            return View(proposals);
        }

        public async Task<IActionResult> MyMatches()
        {
            var matchedProjects = await _context.Proposals
                .Where(p => p.IsIdentityRevealed && p.Status == "Matched")
                .ToListAsync();

            return View(matchedProjects);
        }
    }
}
