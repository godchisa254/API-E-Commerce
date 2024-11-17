using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Dtos.AuthDtos;
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

         public async Task<string?> GetRol(AppUser user)
        {
            var rol = await _userManager.GetRolesAsync(user!);

            return rol.FirstOrDefault();
        }

        public async Task<IActionResult> UpdatePassword( string id, ChangePasswordDto request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return new NotFoundResult();
            }

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

            if (result.Succeeded)
            {
                return new OkResult();
            }

            return new BadRequestObjectResult(result.Errors);
        }


        
    }
}