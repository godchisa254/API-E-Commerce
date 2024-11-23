using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Repository
{
    public class AuthRepository : IAuthRepository
    {

        //Conections
        private readonly ApplicationDBContext _context;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepository(ApplicationDBContext context, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<AppUserDto> GetUserByEmail(string email)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (appUser == null)
            {
                throw new Exception("Usuario no encontrado");;
            }

            AppUserDto appUserDto = appUser.ToUserDto();

            return appUserDto;
            
        }

        public async Task<string?> GetRolbyEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var rol = await _userManager.GetRolesAsync(user!);

            return rol.FirstOrDefault();
        }

        public async Task<IdentityResult> UpdatePassword( string id, ChangePasswordDto request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Usuario no encontrado" });
            }

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(result.Errors.ToArray());
        }

        public async Task<List<AppUser>> GetAllUsers(QueryUser query)
        {
            var pageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var users = _context.Users.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                var normalizedQueryName = query.Name.Trim().ToLower();
                users = users.Where(u => u.Name.ToLower().Contains(normalizedQueryName));
            }
            
            if (query.enabledUser.HasValue)
            {
                if (query.enabledUser.Value) { users = users.Where(u => u.enabledUser); }
                else                         { users = users.Where(u => !u.enabledUser); }
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var propertyInfo = typeof(AppUser).GetProperty(query.SortBy);
                if (propertyInfo != null)
                {
                    users = query.IsDescending ? users.OrderByDescending(x => EF.Property<object>(x, query.SortBy)) : users.OrderBy(x => EF.Property<object>(x, query.SortBy));
                }
                else
                {
                    throw new ArgumentException($"Invalid sort property: {query.SortBy}, Use one of the following: {string.Join(", ", propertyInfo)}");
                }
            }

            var skipNumber = (pageNumber - 1) * pageSize;
            var AppUserModels = await users.Skip(skipNumber).Take(pageSize).ToListAsync();

            return AppUserModels;
        }

        public async Task<AppUser?> GetUserByRut(string rut)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Rut == rut);
        }

        public async Task EnableDisableUser(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUserDto> GetUserByid(string id)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (appUser == null)
            {
                throw new Exception("Usuario no encontrado");;
            }

            AppUserDto appUserDto = appUser.ToUserDto();

            return appUserDto;
            

        }

        public async Task<IdentityResult> EditProfile(string id, EditProfileUserDto request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            //TODO aplicar mapper
            user.Name = request.Name;
            user.Birthdate = request.Birthdate;
            user.Gender = request.Gender;

            await _context.SaveChangesAsync();
            await _userManager.UpdateSecurityStampAsync(user);
            return IdentityResult.Success; 

        }


        public async Task<IdentityResult> DeleteAccount(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return await _userManager.DeleteAsync(user);

        }

        public async Task<IdentityResult> checkPasswordbyId(string id, string newPassword)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(appUser == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var checkPassword= await _signInManager.CheckPasswordSignInAsync(appUser!, newPassword, false);

            return IdentityResult.Success; 

        }

        public async Task<IdentityResult> checkPasswordbyEmail(string email, string password)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);;

            if(appUser == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var checkPassword= await _signInManager.CheckPasswordSignInAsync(appUser!, password, false);

            return IdentityResult.Success; 


        }
    }
}