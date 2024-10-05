using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos;
using taller1.src.Models;


namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegristationDto registerDto)
        {
            try{
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }

                //TODO: crear un exist by code y un exist by email 

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

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(appUser, "User");

                    if(role.Succeeded)
                    {
                        return Ok("User created successfully");
                    }
                    else
                    {
                        return StatusCode(500, role.Errors);
                 
                    }


                } else 
                {
                    return StatusCode(500, createUser.Errors);
                }


            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
                return Ok("AuthController is working");
        }



    }
}