using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UserManagementSystem.Data.Models;

namespace UserManagementSystem.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task UpdateLastLoginDateAsync(this UserManager<User> userManager, User user, DateTime lastLoginDate)
        {
            user.LastLoginDate = lastLoginDate;

            await userManager.UpdateAsync(user);
        }

        public static async Task UpdateStatusAsync(this UserManager<User> userManager, User user, bool isActiveStatus)
        {
            user.IsActive = isActiveStatus;

            await userManager.UpdateAsync(user);
        }
    }
}
