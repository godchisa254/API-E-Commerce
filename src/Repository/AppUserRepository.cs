using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDBContext _context;

        public AppUserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistByRut(string rut)
        {
            return await _context.Users.AnyAsync(u => u.Rut == rut);
        }
    }
}