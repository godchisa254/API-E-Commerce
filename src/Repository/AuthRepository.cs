using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Repository
{
    public class AuthRepository : IAuthRepository
    {

        //Conections
        private readonly ApplicationDBContext _context;

        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> ExistByRut(string rut)
        {
            return await _context.Users.AnyAsync(u => u.Rut == rut);
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddRole(AppUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<AppUser?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}