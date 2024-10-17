using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IAuthRepository
    {
        Task<bool> ExistByRut(string rut);

        Task<IdentityResult> CreateUserAsync(AppUser user, string password);

        Task<IdentityResult> AddRole(AppUser user, string role);

        Task<AppUser?> GetUserByEmail(string email);
    }
}