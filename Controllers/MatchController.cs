using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class MatchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MatchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var proposal = await _context.Proposals.FindAsync(id);

            if (proposal != null && proposal.Status == "Pending")
            {
                proposal.SupervisorName = user.FullName;
                proposal.SupervisorId = user.Id;
                proposal.SupervisorContact = user.Email;

                proposal.Status = "Matched";
                proposal.IsIdentityRevealed = true; 

                await _context.SaveChangesAsync();

                return RedirectToAction("MyMatches", "Supervisor");
            }

            return RedirectToAction("Dashboard", "Supervisor");
        }
    }
}
