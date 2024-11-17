using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface ITokenService
    {
        string CreateTokenUser(AppUser user);

        string CreateTokenAdmin(AppUser admin);
    }
}