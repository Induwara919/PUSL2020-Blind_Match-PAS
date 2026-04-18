using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;

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
        public async Task<IActionResult> Confirm(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var proposal = await _context.Proposals.FindAsync(id);

            if (proposal != null)
            {
                proposal.SupervisorName = user.FullName;
                proposal.SupervisorId = user.Id;
                proposal.SupervisorContact = user.Email;
                proposal.Status = "Matched";
                proposal.IsIdentityRevealed = true; 

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyMatches", "Supervisor");
        }
    }
}