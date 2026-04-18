using Microsoft.AspNetCore.Mvc;
using PUSL2020_Blind_Match_PAS.Services;
using System.Threading.Tasks;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    public class MatchController : Controller
    {
        private readonly ProjectService _projectService;

        public MatchController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            string mockSupervisorId = "SUP-7782";
            string mockSupervisorName = "Dr. Gamage";

            bool success = await _projectService.ConfirmMatchAsync(id, mockSupervisorId, mockSupervisorName);

            if (success)
            {
                return RedirectToAction("MyMatches", "Supervisor");
            }

            return RedirectToAction("Dashboard", "Supervisor");
        }
    }
}