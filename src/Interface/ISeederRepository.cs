using Microsoft.AspNetCore.Identity;
using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para la creación y gestión de usuarios administrativos en el sistema.
    /// </summary>
    public interface ISeederRepository
    {
        /// <summary>
        /// Crea un nuevo usuario administrativo en el sistema con el rol de administrador.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que contiene los datos del usuario administrador a crear.</param>
        /// <param name="password">La contraseña del usuario administrador a crear.</param>
        /// <returns>Devuelve un resultado de identidad <see cref="IdentityResult"/> que indica si la creación fue exitosa.</returns>
        Task<IdentityResult> CreateAdminAsync(AppUser admin, string password);

        /// <summary>
        /// Asocia un rol al usuario administrador especificado.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que representa al usuario administrador al que se le asignará un rol.</param>
        /// <param name="role">El nombre del rol a asignar al usuario administrativo.</param>
        /// <returns>Devuelve un resultado de identidad <see cref="IdentityResult"/> que indica si la asignación del rol fue exitosa.</returns>
        Task<IdentityResult> AddRole(AppUser admin, string role);

        /// <summary>
        /// Obtiene un usuario administrativo basado en su rol.
        /// </summary>
        /// <returns>Devuelve el objeto <see cref="AppUser"/> correspondiente al usuario administrativo si existe, de lo contrario devuelve null.</returns>
        Task<AppUser?> GetAdminByRol();
    }
}
