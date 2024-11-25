using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Dtos.UserDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Repository
{
    /// <summary>
    /// Repositorio para la autenticación de usuarios, gestionando operaciones como creación, modificación y consulta de usuarios.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        // Conexiones
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        /// <summary>
        /// Constructor del repositorio de autenticación.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        /// <param name="userManager">Gestor de usuarios para manejar operaciones relacionadas con usuarios.</param>
        /// <param name="signInManager">Gestor de inicio de sesión para manejar la autenticación de los usuarios.</param>
        public AuthRepository(ApplicationDBContext context, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Verifica si un usuario existe en el sistema por su RUT.
        /// </summary>
        /// <param name="rut">El RUT del usuario.</param>
        /// <returns>Devuelve <c>true</c> si el usuario existe, de lo contrario <c>false</c>.</returns>
        public async Task<bool> ExistByRut(string rut)
        {
            return await _context.Users.AnyAsync(u => u.Rut == rut);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema de autenticación.
        /// </summary>
        /// <param name="user">El objeto <see cref="AppUser"/> que contiene la información del usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Asocia un rol a un usuario en el sistema.
        /// </summary>
        /// <param name="user">El usuario al que se le asignará el rol.</param>
        /// <param name="role">El nombre del rol a asignar al usuario.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>

        public async Task<IdentityResult> AddRole(AppUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Obtiene los detalles de un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Devuelve un objeto <see cref="AppUserDto"/> con los detalles del usuario.</returns>
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

        /// <summary>
        /// Obtiene el rol asociado a un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Devuelve el nombre del rol del usuario o <c>null</c> si no tiene asignado un rol.</returns>
        public async Task<string?> GetRolbyEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email); 
            var rol = await _userManager.GetRolesAsync(user!);

            return rol.FirstOrDefault();
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario en el sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="request">El objeto <see cref="ChangePasswordDto"/> con los detalles de la nueva contraseña.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
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

        /// <summary>
        /// Obtiene una lista de todos los usuarios en el sistema, con posibilidad de filtrado y ordenación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryUser"/> que contiene los filtros y parámetros de paginación.</param>
        /// <returns>Devuelve una lista de objetos <see cref="GetUserDto"/> con los detalles de los usuarios.</returns>
        public async Task<List<GetUserDto>> GetAllUsers(QueryUser query)
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
            var AppUserDto = await users.Skip(skipNumber).Take(pageSize).ToListAsync(); 
            return AppUserDto.Select(u => u.ToGetUserDto()).ToList();
        }

        /// <summary>
        /// Activa o desactiva un usuario en el sistema.
        /// </summary>
        /// <param name="rut">El número de RUT del usuario a activar o desactivar.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task EnableDisableUser(string rut)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Rut == rut);

            if(user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            user.enabledUser = !user.enabledUser; 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene un usuario específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <returns>Devuelve un objeto <see cref="AppUserDto"/> con los detalles del usuario.</returns>
        public async Task<AppUserDto> GetUserByid(string id)
        {
            var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (appUser == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            AppUserDto appUserDto = appUser.ToUserDto(); 
            return appUserDto; 
        }

        /// <summary>
        /// Edita el perfil de un usuario específico.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="request">El objeto <see cref="EditProfileUserDto"/> con los nuevos datos para editar el perfil.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        public async Task<IdentityResult> EditProfile(string id, EditProfileUserDto request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (!string.IsNullOrWhiteSpace(request.Name) || request.Name!.ToLower() != "string")
            {
                user!.Name = request.Name;
            }

            if (request.Birthdate!= null )
            { 
                user!.Birthdate = request.Birthdate!.Value; 
            }

            if (request.Gender != null)
            {
               user!.Gender = request.Gender.Value;
            }

            await _context.SaveChangesAsync();
            await _userManager.UpdateSecurityStampAsync(user!);
            return IdentityResult.Success;  
        }   

        /// <summary>
        /// Elimina una cuenta de usuario del sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario a eliminar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        public async Task<IdentityResult> DeleteAccount(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return await _userManager.DeleteAsync(user); 
        }

        /// <summary>
        /// Verifica si la contraseña actual de un usuario es correcta por su identificador.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que se quiere comprobar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
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

        /// <summary>
        /// Verifica si la contraseña actual de un usuario es correcta por su correo electrónico.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que se quiere comprobar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
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