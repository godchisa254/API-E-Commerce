using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.AuthDtos
{
    public class DeleteAccountDto
    {
        [Required]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Confirmar eliminación")]
        [RegularExpression(@"Confirmo|Rechazo", ErrorMessage = "Confirmacion no valida")]
        public string Confirmation { get; set; } = string.Empty;
    }
}