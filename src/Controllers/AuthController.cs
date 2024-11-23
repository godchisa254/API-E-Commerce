using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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

        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(IAuthRepository authRepository, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _signInManager = signInManager;
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
                        //aplicar mapper
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
                                //eliminar token
                                Token = _tokenService.CreateTokenUser(appUser)
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = await _authRepository.GetUserByEmail(loginDto.Email);

                if(appUser == null)
                {
                    return NotFound("Correo o Contraseña Invalidos");
                }   

                var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

                if(!result.Succeeded || appUser == null)
                {
                    return Unauthorized("Correo o Contraseña Invalidos");
                }
                
                if(!appUser.enabledUser)
                {
                    return Unauthorized("Usuario deshabilitado, inicio de sesión no permitido.");
                }

                string? appRol = await _authRepository.GetRol(appUser);

                string createToken;

                if( appRol == "Admin")
                {
                    createToken = _tokenService.CreateTokenAdmin(appUser);
                }
                else
                {
                    createToken = _tokenService.CreateTokenUser(appUser);
                }

                return Ok(
                    new NewUserDto
                    {
                        //aplicar mapper
                        Rut = appUser.Rut!,
                        Name = appUser.Name!,
                        Email = appUser.Email!,
                        Token = createToken
                    }
                );


            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("actualizar-contrasena")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDto newPasswordDto)
        {
            //aplicar try and catch
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;


            var userId = userIdClaim.Value;

            var user = await _authRepository.GetUserByid(userId);

            if (user == null)
            {
            return NotFound("Usuario no encontrado");
            }
    
            var checkPassword= await _signInManager.CheckPasswordSignInAsync(user!, newPasswordDto.Password, false);

            if(!checkPassword.Succeeded)
            {
                return Unauthorized("Contraseña Invalida");
            }

            if(newPasswordDto.Password == newPasswordDto.NewPassword)
            {
                return BadRequest("La nueva contraseña no puede ser igual a la anterior");
            }

            if(newPasswordDto.NewPassword != newPasswordDto.ConfirmNewPassword)
            {
                return BadRequest("La nueva contraseña debe de coincidir con su confirmacion");
            }

            var result = await _authRepository.UpdatePassword(userId, newPasswordDto);

            if(result.Succeeded)
            {

                var newToken = _tokenService.CreateTokenUser(user!);

                
                var Response = new {

                    Message = "Contraseña actualizada correctamente",
                    token = newToken
                };
                
                return Ok(Response);

            }


            return BadRequest("Fallo al actualizar contraseña");
            
        }

        [HttpPut("editar-perfil")]
        [Authorize]
        public async Task<IActionResult> EditProfileUser([FromBody] EditProfileUserDto editProfileUserDto)
        {

            try{

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;


                var userId = userIdClaim.Value;


                var result = await _authRepository.EditProfile(userId, editProfileUserDto);


                if(result.Succeeded)
                {

                    var appUser = await _authRepository.GetUserByid(userId);

                    return Ok(
                    new  {
                        Message = "Perfil editado correctamente",


                        UpdateUser = new EditProfileUserDto {
                        //aplicar mapper
                        Name = appUser!.Name,
                        Birthdate = appUser.Birthdate,
                        Gender = appUser.Gender

                        },

                        newToken = _tokenService.CreateTokenUser(appUser!)

                        }
                    );
                }

                return BadRequest("Edicion de perfil fallida");


            }catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }




        }


        [HttpDelete("eliminar-cuenta")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> DeleteProfileUser([FromBody] DeleteAccountDto deleteDto)
        {

            try{

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;


                var userId = userIdClaim.Value;

                var user = await _authRepository.GetUserByid(userId);
    
                var checkPassword= await _signInManager.CheckPasswordSignInAsync(user!, deleteDto.Password, false);

                if(!checkPassword.Succeeded)
                {
                    return Unauthorized("Contraseña Invalida");
                }

                if(deleteDto.Confirmation.ToLower() != "confirmo")
                {
                    return BadRequest("Eliminacion rechazada");
                }

                var result = await _authRepository.DeleteAccount(userId);

                if(result != null)
                {
                
                    return Ok("Cuenta eliminada correctamente");

                }

                return BadRequest("Fallo al eliminar cuenta");




            }catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    



    }
}