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

        public async Task<bool> CreateUserAsync(AppUser user, string password)
        {
            var resultCreate = await _userManager.CreateAsync(user, password);
            return resultCreate.Succeeded;
        }

        public async Task<bool> AddRole(AppUser user, string role)
        {
            var resultRole = await _userManager.AddToRoleAsync(user, role);
            return resultRole.Succeeded;
        }
    }
}