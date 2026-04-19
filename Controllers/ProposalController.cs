using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProposalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProposalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View("~/Views/Proporsal/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectProposal proposal)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                proposal.StudentName = user.FullName;
                proposal.StudentId = user.StudentId;

                proposal.Status = "Pending";
                proposal.IsIdentityRevealed = false; 

                _context.Add(proposal);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyProposals));
            }
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View("~/Views/Proporsal/Create.cshtml", proposal);
        }

        public async Task<IActionResult> MyProposals()
        {
            var user = await _userManager.GetUserAsync(User);

            var proposals = await _context.Proposals
                .Where(p => p.StudentId == user.StudentId)
                .ToListAsync();

            return View("~/Views/Proporsal/MyProposals.cshtml", proposals);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null) return NotFound();

            if (proposal.Status == "Matched")
            {
                return BadRequest("Cannot edit a proposal that has already been matched.");
            }

            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View("~/Views/Proporsal/Edit.cshtml", proposal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectProposal proposal)
        {
            if (id != proposal.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Proposals.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

                    if (original.Status == "Matched") return BadRequest();

                    proposal.StudentName = original.StudentName;
                    proposal.StudentId = original.StudentId;
                    proposal.Status = original.Status;
                    proposal.IsIdentityRevealed = original.IsIdentityRevealed;

                    _context.Update(proposal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Proposals.Any(e => e.Id == proposal.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(MyProposals));
            }
            ViewBag.Tags = await _context.Tags.ToListAsync();
            return View("~/Views/Proporsal/Edit.cshtml", proposal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                if (proposal.Status != "Matched")
                {
                    _context.Proposals.Remove(proposal);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(MyProposals));
        }
    }
}
