using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    public class ProposalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProposalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View("~/Views/Proporsal/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectProposal proposal)
        {
            if (ModelState.IsValid)
            {
                proposal.Status = "Pending";
                proposal.IsIdentityRevealed = false;

                _context.Add(proposal);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyProposals));
            }
            return View("~/Views/Proporsal/Create.cshtml", proposal);
        }

        public async Task<IActionResult> MyProposals()
        {
            var proposals = await _context.Proposals.ToListAsync();
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
