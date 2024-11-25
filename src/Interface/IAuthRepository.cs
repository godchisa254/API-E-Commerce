using Microsoft.AspNetCore.Identity;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Dtos.UserDtos;
using taller1.src.Helpers;
using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para la intervención en la base de datos con la autenticación y usuarios en el sistema.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Verifica si un usuario existe en el sistema por su RUT.
        /// </summary>
        /// <param name="rut">El RUT del usuario.</param>
        /// <returns>Devuelve <c>true</c> si el usuario existe, de lo contrario <c>false</c>.</returns>
        Task<bool> ExistByRut(string rut);

        /// <summary>
        /// Crea un nuevo usuario en el sistema de autenticación.
        /// </summary>
        /// <param name="user">El objeto <see cref="AppUser"/> que contiene la información del usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> CreateUserAsync(AppUser user, string password);

        /// <summary>
        /// Asocia un rol a un usuario en el sistema.
        /// </summary>
        /// <param name="user">El usuario al que se le asignará el rol.</param>
        /// <param name="role">El nombre del rol a asignar al usuario.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> AddRole(AppUser user, string role);

        /// <summary>
        /// Obtiene los detalles de un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Devuelve un objeto <see cref="AppUserDto"/> con los detalles del usuario.</returns>
        Task<AppUserDto> GetUserByEmail(string email);

        /// <summary>
        /// Obtiene el rol asociado a un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Devuelve el nombre del rol del usuario o <c>null</c> si no tiene asignado un rol.</returns>
        Task<string?> GetRolbyEmail(string email);

        /// <summary>
        /// Actualiza la contraseña de un usuario en el sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="request">El objeto <see cref="ChangePasswordDto"/> con los detalles de la nueva contraseña.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> UpdatePassword(string id, ChangePasswordDto request);

        /// <summary>
        /// Obtiene una lista de todos los usuarios en el sistema, con posibilidad de filtrado y ordenación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryUser"/> que contiene los filtros y parámetros de paginación.</param>
        /// <returns>Devuelve una lista de objetos <see cref="GetUserDto"/> con los detalles de los usuarios.</returns>
        Task<List<GetUserDto>> GetAllUsers(QueryUser query);

        /// <summary>
        /// Activa o desactiva un usuario en el sistema.
        /// </summary>
        /// <param name="rut">El número de RUT del usuario a activar o desactivar.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        Task EnableDisableUser(string rut);

        /// <summary>
        /// Obtiene un usuario específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <returns>Devuelve un objeto <see cref="AppUserDto"/> con los detalles del usuario.</returns>
        Task<AppUserDto> GetUserByid(string id);

        /// <summary>
        /// Edita el perfil de un usuario específico.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="request">El objeto <see cref="EditProfileUserDto"/> con los nuevos datos para editar el perfil.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> EditProfile(string id, EditProfileUserDto request);

        /// <summary>
        /// Elimina una cuenta de usuario del sistema.
        /// </summary>
        /// <param name="id">El identificador único del usuario a eliminar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> DeleteAccount(string id);

        /// <summary>
        /// Verifica si la contraseña actual de un usuario es correcta por su identificador.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que se quiere comprobar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> checkPasswordbyId(string id, string newPassword);

        /// <summary>
        /// Verifica si la contraseña actual de un usuario es correcta por su correo electrónico.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que se quiere comprobar.</param>
        /// <returns>Devuelve un <see cref="IdentityResult"/> con el resultado de la operación.</returns>
        Task<IdentityResult> checkPasswordbyEmail(string id, string newPassword);
    }
}
