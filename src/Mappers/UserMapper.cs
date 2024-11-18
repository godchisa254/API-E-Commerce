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
    }

}