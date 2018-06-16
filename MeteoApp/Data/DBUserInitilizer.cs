using Microsoft.AspNetCore.Identity;
using MeteoApp.Common;
using MeteoApp.Models;
using System.Linq;

namespace MeteoApp.Data
{
    public class DBUserInitilizer
    {
        private readonly UserDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBUserInitilizer(
            UserDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //This example just creates an Administrator role and one Admin users
        public async void Initialize()
        {
            //create database schema if none exists
            _context.Database.EnsureCreated();

            //If there is already an Administrator role, abort
            if (_context.Roles.Any(r => r.Name == "Administrator")) return;

            //Create the Administartor Role
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));

            //Create the default Admin account and apply the Administrator role
            string user = Constants.AdminId;
            string password = "1234";
            await _userManager.CreateAsync(new ApplicationUser { UserName = user, Email = user, EmailConfirmed = true }, password);
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user), "Administrator");
        }
    }
}

