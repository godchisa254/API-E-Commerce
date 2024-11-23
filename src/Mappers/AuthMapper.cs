using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    public static class AuthMapper
    {
        // AppUserDto to appUser
        public static AppUser ToUser(this AppUserDto userDto)
        {
            return new AppUser
            {
                Id = userDto.Id,
                Rut = userDto.Rut,
                Name = userDto.Name,
                Email = userDto.Email,
            };
        }

        // AppUser to AppUserDto
        public static AppUserDto ToUserDto(this AppUser user)
        {
            return new AppUserDto
            {
                Id = user.Id,
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
            };

        }    


    }
}