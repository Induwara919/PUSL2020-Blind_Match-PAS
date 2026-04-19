using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    [Authorize] 
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Supervisor"))
            {
                return RedirectToAction("Dashboard", "Supervisor");
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("MyProposals", "Proposal");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
