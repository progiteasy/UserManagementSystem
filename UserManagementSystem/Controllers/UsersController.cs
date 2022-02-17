using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementSystem.Data.Models;
using UserManagementSystem.Extensions;
using UserManagementSystem.ViewModels.Users;

namespace UserManagementSystem.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private async Task<IActionResult> ChangeStatusAsync(IEnumerable<UserViewModel> userModels, bool isActiveStatus)
        {
            var selectedUserModels = userModels.Where(userModel => userModel.IsSelected);

            foreach (var userModel in selectedUserModels)
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);

                if (user == null)
                    continue;

                if ((isActiveStatus && !user.IsActive) || (!isActiveStatus && user.IsActive))
                {
                    if (!isActiveStatus)
                        await _userManager.UpdateSecurityStampAsync(user);
                    await _userManager.UpdateStatusAsync(user, isActiveStatus);
                }
            }

            return Redirect("~/users");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetIndexViewAsync()
        {
            var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (loggedInUser == null || !loggedInUser.IsActive)
                return Redirect("~/account/login");

            await _userManager.UpdateLastLoginDateAsync(loggedInUser, DateTime.Now);

            var users = await _userManager.Users.ToListAsync();
            var userModels = users.Select(user => new UserViewModel()
            {
                IsSelected = false,
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RegistrationDate = user.RegistrationDate.ToString(),
                LastLoginDate = user.LastLoginDate != DateTime.MinValue ?
                    user.LastLoginDate.ToString() : "No data",
                Status = user.IsActive ? "Active" : "Blocked"
            }).ToList();

            return View("IndexView", userModels);
        }

        [HttpPost("block")]
        [Authorize]
        public async Task<IActionResult> BlockAsync(IEnumerable<UserViewModel> userModels)
            => await ChangeStatusAsync(userModels, false);

        [HttpPost("unblock")]
        [Authorize]
        public async Task<IActionResult> UnblockAsync(IEnumerable<UserViewModel> userModels)
            => await ChangeStatusAsync(userModels, true);

        [HttpPost("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(IEnumerable<UserViewModel> userModels)
        {
            var selectedUserModels = userModels.Where(userModel => userModel.IsSelected);
            
            foreach (var userModel in selectedUserModels)
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);

                if (user == null)
                    continue;

                await _userManager.DeleteAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
            }

            return Redirect("~/users");
        }
    }
}
