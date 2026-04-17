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
            return View();
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
            return View(proposal);
        }

        
        public async Task<IActionResult> MyProposals()
        {
           
            var proposals = await _context.Proposals.ToListAsync();

            return View(proposals);
        }
    }
}