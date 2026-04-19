using PUSL2020_Blind_Match_PAS.Data;
using PUSL2020_Blind_Match_PAS.Models;
using Microsoft.EntityFrameworkCore;

namespace PUSL2020_Blind_Match_PAS.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ConfirmMatchAsync(int proposalId, ApplicationUser supervisor)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);

            if (proposal == null || proposal.Status == "Matched") return false;

            proposal.SupervisorId = supervisor.Id;
            proposal.SupervisorName = supervisor.FullName;
            proposal.SupervisorContact = supervisor.Email;

            proposal.Status = "Matched";
            proposal.IsIdentityRevealed = true; 

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetMatchAsync(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);

            if (proposal == null) return false;

            proposal.Status = "Pending";
            proposal.IsIdentityRevealed = false; 
            proposal.SupervisorName = null;
            proposal.SupervisorId = null;
            proposal.SupervisorContact = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
