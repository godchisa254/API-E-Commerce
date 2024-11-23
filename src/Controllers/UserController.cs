using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public UserController(IAuthRepository authRepository)
        {
            _authRepository = authRepository; 
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] QueryUser query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var validSortProperties = new[] { "enabledUser", "Name"};
                if (!validSortProperties.Contains(query.SortBy))
                {
                    return BadRequest($"Invalid SortBy property: {query.SortBy}. Use one of the following: {string.Join(", ", validSortProperties)}");
                }
            }

            var users = await _authRepository.GetAllUsers(query);
            return Ok(users);
        }

        [HttpPut("{rut}")]
        public async Task<IActionResult> EnableDisableUser(string rut)
        {
            try {

                await _authRepository.EnableDisableUser(rut);
                return Ok("Usuario Actualizado" );

            } catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}