
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Helpers;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IAuthRepository
    {
        Task<bool> ExistByRut(string rut);

        Task<IdentityResult> CreateUserAsync(AppUser user, string password);

        Task<IdentityResult> AddRole(AppUser user, string role);

        Task<AppUser?> GetUserByEmail(string email);

        Task<string?> GetRol(AppUser user);

        Task<IdentityResult> UpdatePassword(string id, ChangePasswordDto request);

        Task<List<AppUser>> GetAllUsers(QueryUser query);

        Task<AppUser?> GetUserByRut(string rut);

        Task EnableDisableUser(AppUser user);

        Task<AppUser?> GetUserByid(string id);

        Task<AppUser> EditProfile(string id, EditProfileUserDto request);

        Task<AppUser?> DeleteAccount(string id);
    }
}