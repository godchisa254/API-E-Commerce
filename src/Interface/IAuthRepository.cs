using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IAuthRepository
    {
        Task<bool> ExistByRut(string rut);

        Task<bool> CreateUserAsync(AppUser user, string password);

        Task<bool> AddRole(AppUser user, string role);
    }
}