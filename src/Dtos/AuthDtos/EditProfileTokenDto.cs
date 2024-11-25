using System.ComponentModel.DataAnnotations;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) utilizado para mostrar al usuario la respuesta de la edicion de perfil.
    /// </summary>
    public class EditProfileTokenDto
    {
        /// <summary>
        /// El nombre del usuario, requerido y con una longitud máxima de 255 caracteres.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// La fecha de nacimiento del usuario, un valor requerido.
        /// </summary>
        [Required]
        public DateOnly Birthdate { get; set; }

        /// <summary>
        /// Género del usuario. El valor debe estar en el rango de 0 a 4, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decir. 
        /// </summary>
        [Required]
        [Range(0, 4)]
        public int Gender { get; set; }

        /// <summary>
        /// El token JWT generado para el usuario después de la edición de su perfil.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}