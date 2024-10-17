using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Interface;
using taller1.src.Models;


namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        private readonly ITokenService _tokenService;

        public AuthController(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
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
                    return BadRequest("La contraseña es requerida");
                }

                if(registerDto.Password != registerDto.ConfirmPassword)
                {
                    return BadRequest("La contraseña y Confirmacion de contraseña deben ser iguales");
                }

                var createUser = await _authRepository.CreateUserAsync(appUser, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var role = await _authRepository.AddRole(appUser, "User");

                    if(role.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                Rut = appUser.Rut,
                                Name = appUser.Name,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, role.Errors.Select(e => e.Description));
                 
                    }


                } else 
                {

                    var errors = createUser.Errors.Select(e => e.Description);

                    if(errors.Any(e => e.Contains("Username")))
                    {
                        errors = errors.Select(e => e.Replace("Username", "Email")).ToList();
                    }

                    return StatusCode(500, errors);
                }


            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        
    }
}