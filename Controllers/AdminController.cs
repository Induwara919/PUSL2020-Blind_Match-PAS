using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public async Task<IActionResult> ManageTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return View(tags);
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                _context.Tags.Add(new Tag { Name = tagName });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageTags));
        }

        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reassign(int id, string newSupervisorName)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.SupervisorName = newSupervisorName;
                proposal.Status = "Matched";
                proposal.IsIdentityRevealed = true; 
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ResetMatch(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.Status = "Pending";
                proposal.IsIdentityRevealed = false;
                proposal.SupervisorName = null;
                proposal.SupervisorId = null;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
