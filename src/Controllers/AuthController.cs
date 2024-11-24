using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using taller1.src.Dtos;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;


namespace taller1.src.Controllers
{
    /// <summary>
    /// Controlador para la autenticación y gestión de usuarios.
    /// Proporciona métodos para registro, inicio de sesión, actualización de contraseñas, edición de perfil y eliminación de cuentas.
    /// </summary>
    /// 
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Patron repostory de Auth
        /// </summary>
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// Servicio para generacion de jtoken JWT .
        /// </summary>
        private readonly ITokenService _tokenService;


        /// <summary>
        /// Constructor del AuthController.
        /// </summary>
        /// <param name="authRepository">Repositorio de Auth.</param>
        /// <param name="tokenService">Servicio para la generacion de tokens JWT.</param>
        public AuthController(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }
        

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="registerDto">DTO de registro del usuario. Para más detalles, ver <see cref="UserRegristationDto"/>.</param>
        /// <returns>Respuesta con el usuario creado o un mensaje de error.</returns>
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

                if(registerDto.Birthdate.Year >= DateTime.Now.Year)
                {
                    return BadRequest("La fecha de nacimiento debe ser menor a la actual");
                }

                AppUser appUser = registerDto.ToUserRegister();


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
 
                        NewUserDto newUser = appUser.ToUserNewUserDto();

                        return Ok(newUser);

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


        /// <summary>
        /// Inicia sesión en el sistema con credenciales del usuario.
        /// </summary>
        /// <param name="loginDto">DTO de inicio de sesión del usuario. Para más detalles, ver <see cref="LoginDto"/>.</param>
        /// <returns>Respuesta con el token de sesión y los detalles del usuario, o un mensaje de error.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUserDto = await _authRepository.GetUserByEmail(loginDto.Email);

                AppUser appUser = appUserDto.ToUser();

                checkLoginDto checkLogin = appUser.ToCheckLoginDto();


                if(checkLogin == null)
                {
                    return NotFound("Correo o Contraseña Invalidos");
                }   

                if(!checkLogin.enabledUser)
                {
                    return Unauthorized("Usuario deshabilitado");
                }

                var result = await _authRepository.checkPasswordbyEmail(loginDto.Email, loginDto.Password);

                if(!result.Succeeded || appUser == null)
                {
                    return Unauthorized("Correo o Contraseña Invalidos");
                }
                
                if(!appUser.enabledUser)
                {
                    return Unauthorized("Usuario deshabilitado, inicio de sesión no permitido.");
                }


                string? appRol = await _authRepository.GetRolbyEmail(loginDto.Email);

                string createToken;

                if( appRol == "Admin")
                {
                    createToken = _tokenService.CreateTokenAdmin(appUser);
                }
                else
                {
                    createToken = _tokenService.CreateTokenUser(appUser);
                }

                NewUserLoginDto newUser = appUser.toNewUserLoginDto();

                newUser.Token = createToken;

                return Ok(newUser);


            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        /// <summary>
        /// Actualiza la contraseña del usuario autenticado.
        /// </summary>
        /// <param name="newPasswordDto">DTO con las contraseñas nueva y actual del usuario. Para más detalles, ver <see cref="ChangePasswordDto"/>.</param>
        /// <returns>Respuesta indicando éxito o error en la actualización.</returns>
        [HttpPut("actualizar-contrasena")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordDto newPasswordDto)
        {

            try{

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;


                var userId = userIdClaim.Value;


                var checkPassword = await _authRepository.checkPasswordbyId(userId, newPasswordDto.Password);



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

                    var user = await _authRepository.GetUserByid(userId);

                    AppUser appUser = user.ToUser();

                    var newToken = _tokenService.CreateTokenUser(appUser!);

                
                    var Response = new {

                    Message = "Contraseña actualizada correctamente",
                    token = newToken
                    };
                
                    return Ok(Response);

                }


                return BadRequest("Fallo al actualizar contraseña");

            }catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
            


            
        }


        /// <summary>
        /// Edita el perfil del usuario autenticado.
        /// </summary>
        /// <param name="editProfileUserDto">DTO con los datos del perfil a actualizar. Para más detalles, ver <see cref="EditProfileUserDto"/>.</param>
        /// <returns>Respuesta con el perfil actualizado y un nuevo token de sesión, o un mensaje de error.</returns>
        /// <remarks>
        /// Si no se desea cambiar algún campo, se deberá asignar un valor <c>null</c> a dicho campo. 
        /// </remarks>
        [HttpPut("editar-perfil")]
        [Authorize]
        public async Task<IActionResult> EditProfileUser([FromBody] EditProfileUserDto editProfileUserDto)
        {

            try{

                
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;

                var userId = userIdClaim.Value;

                var result = await _authRepository.EditProfile(userId, editProfileUserDto);


                if(result.Succeeded)
                {

                    var userDto = await _authRepository.GetUserByid(userId);

                    AppUser appUser = userDto.ToUser();

                    var newToken = _tokenService.CreateTokenUser(appUser!);

                    EditProfileTokenDto editUser = appUser.ToUserEditProfileToken();
                    editUser.Token = newToken;


                    return Ok(
                    new  {
                        Message = "Perfil editado correctamente",

                        editUser

                        }
                    );
                }

                return BadRequest("Edicion de perfil fallida");


            }catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


            

        }

  
        /// <summary>
        /// Elimina la cuenta del usuario autenticado.
        /// </summary>
        /// <param name="deleteDto">DTO con la contraseña y confirmación para eliminar la cuenta. Para más detalles, ver <see cref="DeleteAccountDto"/>.</param>
        /// <returns>Respuesta indicando éxito o error en la eliminación.</returns>
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
    
                var checkPassword = await _authRepository.checkPasswordbyId(userId, deleteDto.Password);

                if(!checkPassword.Succeeded)
                {
                    return Unauthorized("Contraseña Invalida");
                }

                if(deleteDto.Confirmation.ToLower() != "confirmo")
                {
                    return BadRequest("Eliminacion rechazada");
                }

                var result = await _authRepository.DeleteAccount(userId);

                if(result.Succeeded)
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