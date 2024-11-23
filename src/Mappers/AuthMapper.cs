using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos;
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
                enabledUser = userDto.enabledUser
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
                enabledUser = user.enabledUser
            };

        }    


        public static AppUser toUserCheckLogin(this checkLoginDto userDto)
        {
            return new AppUser
            {
                Rut = userDto.Rut,
                Name = userDto.Name,
                Email = userDto.Email,
                enabledUser = userDto.enabledUser,
            };
        }

        public static checkLoginDto ToCheckLoginDto(this AppUser user)
        {
            return new checkLoginDto
            {
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
                enabledUser = user.enabledUser,
            };
        }

        public static AppUser ToNewUserLogin(this NewUserLoginDto loginDto)
        {
            return new AppUser
            {
                Rut = loginDto.Rut,
                Name = loginDto.Name,
                Email = loginDto.Email,
                Token = loginDto.Token,
            };
        }

        public static NewUserLoginDto toNewUserLoginDto(this AppUser user)
        {
            return new NewUserLoginDto
            {
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
                Token = user.Token,
            };
        }

        public static AppUser ToUserRegister(this UserRegristationDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Rut = registerDto.Rut,
                Name = registerDto.Name,
                Birthdate = registerDto.Birthdate,
                Gender= registerDto.Gender
            };
        }

        public static NewUserDto ToUserNewUserDto (this AppUser user)
        {
            return new NewUserDto
            {
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
                
            };

        }    

        public static EditProfileTokenDto ToUserEditProfileToken(this AppUser user)
        {
            return new EditProfileTokenDto
            {
                Name = user.Name,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                Token = user.Token

            };    
        }


    }
}