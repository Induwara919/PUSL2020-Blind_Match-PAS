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

        public async Task<bool> ConfirmMatchAsync(int proposalId, string supervisorId, string supervisorName)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);

            if (proposal == null) return false;

            proposal.SupervisorId = supervisorId;
            proposal.SupervisorName = supervisorName;
            proposal.Status = "Matched";
            proposal.IsIdentityRevealed = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}