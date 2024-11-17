using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.AuthDtos
{
    public class ChangePasswordDto
    {

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string NewPassword { get; set; } = string.Empty;


        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}