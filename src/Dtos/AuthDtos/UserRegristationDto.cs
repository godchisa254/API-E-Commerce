using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Dtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) que muestra al usuario los datos a rellenar para registrarse.
    /// </summary>
    public class UserRegristationDto
    {
        /// <summary>
        /// RUT del usuario. Este campo es obligatorio y debe tener un máximo de 12 caracteres.
        /// </summary>
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario. Este campo es obligatorio y debe tener un máximo de 255 caracteres.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del usuario, campo obligatorio.
        /// </summary>
        [Required]
        public DateOnly Birthdate { get; set; }

         /// <summary>
        /// Dirección de correo electrónico del usuario. Este campo es obligatorio, debe ser válido y tener un máximo de 255 caracteres.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Género del usuario. El valor debe estar en el rango de 0 a 3, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decirlo. 
        /// </summary>
        [Required]
        [Range(0, 4)]
        public int Gender { get; set; } 

        /// <summary>
        /// La contraseña del usuario. Este campo es obligatorio y debe cumplir con los siguientes requisitos:
        /// - Longitud mínima de 8 caracteres.
        /// - Longitud máxima de 20 caracteres.
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Confirmación de la nueva contraseña, Este campo es obligatorio.
        /// </summary>
        /// <remarks>
        /// Este campo debe coincidir con el campo de Password
        /// </remarks>
        [Required]
        [Column(TypeName = "varchar")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}