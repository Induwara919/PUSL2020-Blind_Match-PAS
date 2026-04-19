using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using PUSL2020_Blind_Match_PAS.Controllers;

namespace PUSL2020_Blind_Match_PAS.Tests.UnitTests
{
    public class HomeControllerTests
    {
        [Theory]
        [InlineData("Admin", "Index", "Admin")]
        [InlineData("Supervisor", "Dashboard", "Supervisor")]
        [InlineData("Student", "MyProposals", "Proposal")]
        public void Index_RedirectsToCorrectDashboard_BasedOnRole(string role, string expectedAction, string expectedController)
        {
            var controller = new HomeController();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Role, role)
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = controller.Index() as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(expectedController, result.ControllerName);
        }
    }
}