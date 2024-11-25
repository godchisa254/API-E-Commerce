using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos para crear tokens de autenticación para usuarios y administradores.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Crea un token JWT para un usuario.
        /// </summary>
        /// <param name="user">El objeto <see cref="AppUser"/> que representa al usuario para el cual se generará el token.</param>
        /// <returns>Devuelve un token JWT en formato de cadena para el usuario especificado.</returns>
        string CreateTokenUser(AppUser user);

        /// <summary>
        /// Crea un token JWT para un administrador.
        /// </summary>
        /// <param name="admin">El objeto <see cref="AppUser"/> que representa al administrador para el cual se generará el token.</param>
        /// <returns>Devuelve un token JWT en formato de cadena para el administrador especificado.</returns>
        string CreateTokenAdmin(AppUser admin);
    }
}
