using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos;
using taller1.src.Interface;
using taller1.src.Models;


namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegristationDto registerDto)
        {
            try{

                bool exist = await _authRepository.ExistByRut(registerDto.Rut);
                if(exist)
                {
                    return StatusCode(409, "El rut ingresado ya existe");
                }
                else if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }


                var appUser = new AppUser
                    {
                        UserName = registerDto.Email,  
                        Email = registerDto.Email,
                        Rut = registerDto.Rut,
                        Name = registerDto.Name,  
                        Birthdate = registerDto.Birthdate,
                        Gender = registerDto.Gender
                    };

                if(string.IsNullOrEmpty(registerDto.Password))
                {
                    return BadRequest("Password is requerid");
                }

                var createUser = await _authRepository.CreateUserAsync(appUser, registerDto.Password);

                if(createUser)
                {
                    var role = await _authRepository.AddRole(appUser, "User");

                    if(role)
                    {
                        return Ok("User created successfully");
                    }
                    else
                    {
                        return StatusCode(500);
                 
                    }


                } else 
                {
                    return StatusCode(500);
                }


            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}