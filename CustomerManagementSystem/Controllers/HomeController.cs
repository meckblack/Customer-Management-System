using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementSystem.Data;
using CustomerManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerManagementSystem.Models;
using CustomerManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CustomerManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            await SeedSystemAdmin();
            return View();
        }
        
        public async Task SeedSystemAdmin()
        {
            var roleExist = await _dbContext.Roles.AnyAsync(n => n.Name == "System Administrator");
            var password = "Password123#";
            if (roleExist == false)
            {
                var globalRole = new ApplicationRole
                {
                    Name = "System Administrator",
                    NormalizedName = "System Administrator".ToUpper(),
                    Access = "",
                    AddedDate = DateTime.Now,
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                };
                await _dbContext.Roles.AddAsync(globalRole);
                await _dbContext.SaveChangesAsync();
                var hasher = new PasswordHasher<ApplicationUser>();
                var admin = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@test.com",
                    NormalizedUserName = "Admin".ToUpper(),
                    NormalizedEmail = "admin@test.com".ToUpper(),
                    Name = "System Admin",
                    PhoneNumber = "1234567890",
                    PasswordHash = hasher.HashPassword(null, password),
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                };
                await _dbContext.Users.AddAsync(admin);
                await _dbContext.SaveChangesAsync();
                var userRole = new IdentityUserRole<int>
                {
                    RoleId = globalRole.Id,
                    UserId = admin.Id
                };
                await _dbContext.UserRoles.AddAsync(userRole);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}