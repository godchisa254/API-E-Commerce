using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) se utiliza para mostrar respuesta al momento de registrar un usuario.
    /// </summary>
    public class NewUserDto
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
        /// Dirección de correo electrónico del usuario. Este campo es obligatorio, debe ser válido y tener un máximo de 255 caracteres.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

    }
}