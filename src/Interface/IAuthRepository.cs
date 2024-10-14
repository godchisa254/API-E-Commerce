using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Interface
{
    public interface IAuthRepository
    {
        public interface IAppUserRepository
    {
        Task<bool> ExistByRut(string rut);
    }
    }
}