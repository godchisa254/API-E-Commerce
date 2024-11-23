using taller1.src.Dtos.UserDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    public static class UserMapper
    {
        public static GetUserDto ToGetUserDto(this AppUser appUser)
        {
            return new GetUserDto
            {
                Rut = appUser.Rut,
                Name = appUser.Name,
                Birthdate = appUser.Birthdate,
                Gender = appUser.Gender,
                enabledUser = appUser.enabledUser,
                Email = appUser.Email ?? string.Empty
            };
        }

        public static AppUser ToGetUser(this GetUserDto UserDto)
        {
            return new AppUser
            {
                Rut = UserDto.Rut,
                Name = UserDto.Name,
                Birthdate = UserDto.Birthdate,
                Gender = UserDto.Gender,
                enabledUser = UserDto.enabledUser,
                Email = UserDto.Email ?? string.Empty

            };
        }
    }    

}