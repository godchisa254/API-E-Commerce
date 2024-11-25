using taller1.src.Dtos;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Dtos.UserDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    /// <summary>
    /// Proporciona métodos para mapear entre la entidad <see cref="AppUser"/> y sus diferentes DTOs relacionados con la autenticación y usuarios.
    /// </summary>
    public static class AuthMapper
    {
        /// <summary>
        /// Convierte un DTO de usuario en un modelo de usuario.
        /// </summary>
        /// <param name="userDto">El DTO de usuario.</param>
        /// <returns>Un objeto <see cref="AppUser"/> basado en los datos proporcionados.</returns>
        public static AppUser ToUser(this AppUserDto userDto)
        {
            return new AppUser
            {
                Id = userDto.Id,
                Rut = userDto.Rut,
                Name = userDto.Name,
                Email = userDto.Email,
                enabledUser = userDto.enabledUser,
                Birthdate = userDto.Birthdate,
                Gender = userDto.Gender
            };
        }

        /// <summary>
        /// Convierte un modelo de usuario en un DTO de usuario.
        /// </summary>
        /// <param name="user">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="AppUserDto"/> basado en el modelo proporcionado.</returns>
        public static AppUserDto ToUserDto(this AppUser user)
        {
            return new AppUserDto
            {
                Id = user.Id,
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
                enabledUser = user.enabledUser,
                Birthdate = user.Birthdate,
                Gender = user.Gender
            };
        }

        /// <summary>
        /// Convierte un DTO de inicio de sesión en un modelo de usuario.
        /// </summary>
        /// <param name="userDto">El DTO de inicio de sesión.</param>
        /// <returns>Un objeto <see cref="AppUser"/> basado en los datos proporcionados.</returns>
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

        /// <summary>
        /// Convierte un modelo de usuario en un DTO para verificar el inicio de sesión.
        /// </summary>
        /// <param name="user">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="checkLoginDto"/> que contiene los datos del usuario.</returns>
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

        /// <summary>
        /// Convierte un DTO de nuevo inicio de sesión en un modelo de usuario.
        /// </summary>
        /// <param name="loginDto">El DTO de inicio de sesión.</param>
        /// <returns>Un objeto <see cref="AppUser"/> inicializado con los datos proporcionados.</returns>
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

        /// <summary>
        /// Convierte un modelo de usuario en un DTO de nuevo inicio de sesión.
        /// </summary>
        /// <param name="user">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="NewUserLoginDto"/> basado en el modelo proporcionado.</returns>
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

        /// <summary>
        /// Convierte un DTO de registro de usuario en un modelo de usuario.
        /// </summary>
        /// <param name="registerDto">El DTO de registro de usuario.</param>
        /// <returns>Un objeto <see cref="AppUser"/> inicializado con los datos del registro.</returns>
        public static AppUser ToUserRegister(this UserRegristationDto registerDto)
        {
            return new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Rut = registerDto.Rut,
                Name = registerDto.Name,
                Birthdate = registerDto.Birthdate,
                Gender = registerDto.Gender
            };
        }

        /// <summary>
        /// Convierte un modelo de usuario en un DTO de nuevo usuario.
        /// </summary>
        /// <param name="user">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="NewUserDto"/> con los datos del usuario.</returns>
        public static NewUserDto ToUserNewUserDto(this AppUser user)
        {
            return new NewUserDto
            {
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email!,
            };
        }

        /// <summary>
        /// Convierte un modelo de usuario en un DTO de edición de perfil con token.
        /// </summary>
        /// <param name="user">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="EditProfileTokenDto"/> basado en el usuario.</returns>
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
        
        /// <summary>
        /// Convierte un modelo de usuario en un DTO de usuario general.
        /// </summary>
        /// <param name="appUser">El modelo de usuario.</param>
        /// <returns>Un objeto <see cref="GetUserDto"/> que representa al usuario.</returns>
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
