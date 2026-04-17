using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using System.Threading.Tasks;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allProposals = await _context.Proposals.ToListAsync();
            return View(allProposals);
        }
        [HttpPost]
        public async Task<IActionResult> ResetStatus(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.Status = "Pending";
                proposal.IsIdentityRevealed = false;
                proposal.SupervisorId = null;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
