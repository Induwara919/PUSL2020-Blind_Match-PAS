using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PUSL2020_Blind_Match_PAS.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupervisorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard(string selectedArea)
        {
            ViewBag.AvailableTags = await _context.Tags.Select(t => t.Name).ToListAsync();

            var query = _context.Proposals.Where(p => !p.IsIdentityRevealed && p.Status == "Pending");

            if (!string.IsNullOrEmpty(selectedArea))
            {
                query = query.Where(p => p.ResearchArea == selectedArea);
                ViewBag.SelectedArea = selectedArea;
            }

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> MyMatches()
        {
            var user = await _userManager.GetUserAsync(User);

            var matchedProjects = await _context.Proposals
                .Where(p => p.IsIdentityRevealed && p.Status == "Matched" && p.SupervisorId == user.Id)
                .ToListAsync();

            return View(matchedProjects);
        }
    }
}
