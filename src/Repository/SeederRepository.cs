using Microsoft.AspNetCore.Identity;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Repository
{
    /// <summary>
    /// Repositorio para el Seeder de la aplicación, gestionando operaciones a ejecutar para una base de datos vacía.
    /// </summary>
    public class SeederRepository : ISeederRepository
    {
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructor del repositorio del Seeder.
        /// </summary>
        /// <param name="userManager">Gestor de usuarios para manejar operaciones relacionadas con usuarios.</param>
        public SeederRepository( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Asocia un rol al usuario administrador especificado.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que representa al usuario administrador al que se le asignará un rol.</param>
        /// <param name="role">El nombre del rol a asignar al usuario administrativo.</param>
        /// <returns>Devuelve un resultado de identidad <see cref="IdentityResult"/> que indica si la asignación del rol fue exitosa.</returns>
        public async Task<IdentityResult> AddRole(AppUser admin, string role)
        {
            return await _userManager.AddToRoleAsync(admin, role);
        }

        /// <summary>
        /// Crea un nuevo usuario administrativo en el sistema con el rol de administrador.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que contiene los datos del usuario administrador a crear.</param>
        /// <param name="password">La contraseña del usuario administrador a crear.</param>
        /// <returns>Devuelve un resultado de identidad <see cref="IdentityResult"/> que indica si la creación fue exitosa.</returns>
        public async Task<IdentityResult> CreateAdminAsync(AppUser admin, string password)
        {
            return await _userManager.CreateAsync(admin, password);
        }

        /// <summary>
        /// Obtiene un usuario administrativo basado en su rol.
        /// </summary>
        /// <returns>Devuelve el objeto <see cref="AppUser"/> correspondiente al usuario administrativo si existe, de lo contrario devuelve null.</returns>
        public async Task<AppUser?> GetAdminByRol()
        {
            var admin = await _userManager.GetUsersInRoleAsync("Admin");

            return admin.FirstOrDefault();
        }
    }
}