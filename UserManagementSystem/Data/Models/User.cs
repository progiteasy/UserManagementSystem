using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagementSystem.Data.Models
{
    public class User : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; }
    }
}
