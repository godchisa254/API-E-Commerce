using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using taller1.src.Data;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Repository
{
    public class SeederRepository : ISeederRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly UserManager<AppUser> _userManager;

        public SeederRepository(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> AddRole(AppUser admin, string role)
        {
            return await _userManager.AddToRoleAsync(admin, role);
        }

        public async Task<IdentityResult> CreateAdminAsync(AppUser admin, string password)
        {
            return await _userManager.CreateAsync(admin, password);
        }

        public async Task<AppUser?> GetAdminByRol()
        {
            var admin = await _userManager.GetUsersInRoleAsync("Admin");

            return admin.FirstOrDefault();
        }
    }
}