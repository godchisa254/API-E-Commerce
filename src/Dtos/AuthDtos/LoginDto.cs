using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) que muestra al usuario los datos a rellenar al momento iniciar sesion.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// El correo electrónico del usuario, utilizado para iniciar sesión. Este campo es obligatorio y 
        /// debe ser una dirección de correo electrónico válida.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// La contraseña del usuario para iniciar sesión. Este campo es obligatorio y no puede superar los 20 caracteres.
        /// </summary>
        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;
    }
}