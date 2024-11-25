using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) que muestra al usuario los datos a rellenar al momento de eliminar cuenta.
    /// </summary>
    public class DeleteAccountDto
    {
        /// <summary>
        /// La contraseña actual del usuario, requerida para confirmar la eliminación de la cuenta.
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// La confirmación de la eliminación de la cuenta. El valor debe ser "Confirmo" para proceder con la eliminación, caso contrario
        /// no se lleva acabo la eliminacion.
        /// </summary>
        [Required]
        [Display(Name = "Confirmar eliminación")]
        [RegularExpression(@"Confirmo|Rechazo", ErrorMessage = "Confirmacion no valida")]
        public string Confirmation { get; set; } = string.Empty;
    }
}