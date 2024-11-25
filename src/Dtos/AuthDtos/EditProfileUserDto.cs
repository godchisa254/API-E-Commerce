using System.ComponentModel.DataAnnotations;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) que muestra al usuario los datos a rellenar al momento de editar perfil.
    /// </summary>
    public class EditProfileUserDto
    {
        /// <summary>
        /// El nombre del usuario. Este campo es opcional, si no se desea cambiar el nombre, se debe dejar en blanco o establecer como null.
        /// </summary>
        [StringLength(255)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// La fecha de nacimiento del usuario. Este campo es opcional, si no se desea cambiar la fecha de nacimiento, se debe dejar como null.
        /// </summary>
        public DateOnly? Birthdate { get; set; }

        /// <summary>
        /// El género del usuario, representado como un valor entero entre 0 y 4, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decirlo.
        /// Este campo es opcional, si no se desea cambiar, se debe dejar como null.
        /// </summary>
        [Range(0, 4)]
        public int? Gender { get; set; }


    }
}