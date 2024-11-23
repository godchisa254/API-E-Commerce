
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

        Task<AppUserDto> GetUserByEmail(string email);

        Task<string?> GetRolbyEmail(string email);

        Task<IdentityResult> UpdatePassword(string id, ChangePasswordDto request);

        Task<List<AppUser>> GetAllUsers(QueryUser query);

        Task<AppUser?> GetUserByRut(string rut);

        Task EnableDisableUser(AppUser user);

        Task<AppUserDto> GetUserByid(string id);

        Task<IdentityResult> EditProfile(string id, EditProfileUserDto request);

        Task<IdentityResult> DeleteAccount(string id);

        Task<IdentityResult> checkPasswordbyId(string id, string newPassword);

        Task<IdentityResult> checkPasswordbyEmail(string id, string newPassword);
    }
}