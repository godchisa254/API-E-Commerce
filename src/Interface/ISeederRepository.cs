using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface ISeederRepository
    {
        Task<IdentityResult> CreateAdminAsync(AppUser admin, string password);

        Task<IdentityResult> AddRole(AppUser admin, string role);

    }
}