using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Helpers;
using taller1.src.Interface;

namespace taller1.src.Controllers
{
    /// <summary>
    /// Controlador que maneja las solicitudes relacionadas con los usuarios. 
    /// Solo los usuarios con el rol de "Admin" pueden acceder a los métodos de este controlador.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// Constructor del controlador de usuarios, que inyecta el repositorio de autenticación.
        /// </summary>
        /// <param name="authRepository">Repositorio que maneja la lógica de autenticación y gestión de usuarios.</param>
        public UserController(IAuthRepository authRepository)
        {
            _authRepository = authRepository; 
        }
        
        /// <summary>
        /// Obtiene todos los usuarios con opciones de filtrado y ordenación.
        /// Solo accesible por administradores. 
        /// </summary>
        /// <param name="query">Parámetros de consulta para filtrar, ordenar y paginar los resultados.</param>
        /// <returns>Una lista de usuarios que cumplen con los criterios de la consulta.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] QueryUser query)
        {
            // Verifica la validez del modelo
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Valida la propiedad de ordenación
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var validSortProperties = new[] { "enabledUser", "Name"};
                if (!validSortProperties.Contains(query.SortBy))
                {
                    return BadRequest($"Invalid SortBy property: {query.SortBy}. Use one of the following: {string.Join(", ", validSortProperties)}");
                }
            }

            // Obtiene los usuarios según la consulta
            var users = await _authRepository.GetAllUsers(query);
            return Ok(users);
        }

        /// <summary>
        /// Habilita o deshabilita a un usuario, según su RUT.
        /// Solo accesible por administradores.
        /// </summary>
        /// <param name="rut">El RUT del usuario que se quiere habilitar o deshabilitar.</param>
        /// <returns>Un mensaje de éxito o un error si la operación falla.</returns>
        [HttpPut("{rut}")]
        public async Task<IActionResult> EnableDisableUser(string rut)
        {
            try
            {
                // Habilita o deshabilita al usuario
                await _authRepository.EnableDisableUser(rut);
                return Ok("Usuario Actualizado");
            } 
            catch (Exception ex)
            {
                // Si ocurre un error, devuelve un mensaje de error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
