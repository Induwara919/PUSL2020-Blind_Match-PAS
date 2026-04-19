using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PUSL2020_Blind_Match_PAS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var proposals = await _context.Proposals.ToListAsync();

            var supervisors = await _userManager.GetUsersInRoleAsync("Supervisor");
            ViewBag.Supervisors = new SelectList(supervisors, "Id", "FullName");

            return View(proposals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSupervisor(int proposalId, string supervisorId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);
            var supervisor = await _userManager.FindByIdAsync(supervisorId);

            if (proposal != null && supervisor != null)
            {
                proposal.SupervisorId = supervisor.Id;
                proposal.SupervisorName = supervisor.FullName;
                proposal.Status = "Matched";
                proposal.IsIdentityRevealed = true; 

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetMatch(int id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal != null)
            {
                proposal.SupervisorId = null;
                proposal.SupervisorName = null;
                proposal.Status = "Pending";
                proposal.IsIdentityRevealed = false; 

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RegisterUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(string fullName, string email, string role, string? studentId, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                StudentId = studentId,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role); 
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        public async Task<IActionResult> ManageTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return View(tags);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageTags(string tagName)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                _context.Tags.Add(new Tag { Name = tagName });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageTags));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageTags));
        }

        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRole = roles.FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, string fullName, string? studentId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            string oldName = user.FullName;
            string? oldStudentId = user.StudentId;

            user.FullName = fullName;
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Student")) { user.StudentId = studentId; }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (roles.Contains("Student"))
                {
                    var studentProposals = _context.Proposals.Where(p => p.StudentId == oldStudentId);

                    foreach (var p in studentProposals)
                    {
                        p.StudentId = studentId;   
                        p.StudentName = fullName; 
                    }
                }

                if (roles.Contains("Supervisor") && oldName != fullName)
                {
                    var supervisorMatches = _context.Proposals.Where(p => p.SupervisorName == oldName);
                    foreach (var p in supervisorMatches) { p.SupervisorName = fullName; }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UsersList));
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Student"))
            {
                var hasProposals = _context.Proposals.Any(p => p.StudentId == user.StudentId);
                if (hasProposals)
                {
                    TempData["ErrorMessage"] = "Deletion Blocked: This student has active proposals. They must withdraw their proposals before the account can be removed.";
                    return RedirectToAction(nameof(UsersList));
                }
            }

            if (roles.Contains("Supervisor"))
            {
                var hasMatches = _context.Proposals.Any(p => p.SupervisorName == user.FullName && p.IsIdentityRevealed);
                if (hasMatches)
                {
                    TempData["ErrorMessage"] = "Deletion Blocked: This supervisor has active project matches. You must reset their matches in the Oversight Dashboard first.";
                    return RedirectToAction(nameof(UsersList));
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Error occurred while deleting the user.";
            }

            return RedirectToAction(nameof(UsersList));
        }
    }
}
