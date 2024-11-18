using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Serialization;

namespace taller1.src.Dtos.AuthDtos
{
    public class ChangePasswordDto
    {

        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string NewPassword { get; set; } = string.Empty;


        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}