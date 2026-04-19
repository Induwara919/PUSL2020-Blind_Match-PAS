using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PUSL2020_Blind_Match_PAS.Controllers;
using PUSL2020_Blind_Match_PAS.Models;
using PUSL2020_Blind_Match_PAS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PUSL2020_Blind_Match_PAS.Tests.AdminTests
{
    public class AdminControllerUserTests
    {
        [Fact]
        public async Task RegisterUser_RedirectsToIndex_WhenCreationSucceeds()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminRegDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

                userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);
                userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ReturnsAsync(IdentityResult.Success);

                var controller = new AdminController(context, userManager.Object);

                var result = await controller.RegisterUser("Banuka Silva", "banuka@nsbm.ac.lk", "Supervisor", null, "Nsbm123!");

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectResult.ActionName);
            }
        }

        [Fact]
        public async Task DeleteUser_BlocksDeletion_IfStudentHasActiveProposals()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminDeleteBlockDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var testStudent = new ApplicationUser { Id = "U1", StudentId = "ST001", Email = "test@nsbm.ac.lk" };
                context.Proposals.Add(new ProjectProposal { Id = 1, StudentId = "ST001", Title = "Test Project" });
                await context.SaveChangesAsync();

                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

                userManager.Setup(x => x.FindByIdAsync("U1")).ReturnsAsync(testStudent);
                userManager.Setup(x => x.GetRolesAsync(testStudent)).ReturnsAsync(new List<string> { "Student" });

                var controller = new AdminController(context, userManager.Object);
                controller.TempData = new Mock<ITempDataDictionary>().Object;

                var result = await controller.DeleteUser("U1");

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("UsersList", redirectResult.ActionName);
                userManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
            }
        }

        [Fact]
        public async Task EditUser_SynchronizesNameChangesAcrossProposals()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminSyncDb").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var studentId = "ST123";
                var studentUser = new ApplicationUser { Id = "U55", FullName = "Old Name", StudentId = studentId };
                context.Proposals.Add(new ProjectProposal { Id = 1, StudentId = studentId, StudentName = "Old Name", Title = "AI Thesis" });
                await context.SaveChangesAsync();

                var userStore = new Mock<IUserStore<ApplicationUser>>();
                var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

                userManager.Setup(x => x.FindByIdAsync("U55")).ReturnsAsync(studentUser);
                userManager.Setup(x => x.GetRolesAsync(studentUser)).ReturnsAsync(new List<string> { "Student" });
                userManager.Setup(x => x.UpdateAsync(studentUser)).ReturnsAsync(IdentityResult.Success);

                var controller = new AdminController(context, userManager.Object);

                await controller.EditUser("U55", "Banuka Silva", studentId);

                var updatedProposal = await context.Proposals.FirstAsync(p => p.Id == 1);
                Assert.Equal("Banuka Silva", updatedProposal.StudentName);
            }
        }
    }
}
