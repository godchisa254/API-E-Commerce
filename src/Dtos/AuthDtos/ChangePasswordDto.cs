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
    /// <summary>
    /// Representa un Data Transfer Object (DTO) que muestra al usuario los datos a rellenar al momento de cambiar la contraseña.
    /// </summary>
    public class ChangePasswordDto
    {

        /// <summary>
        /// Contraseña actual del usuario. Este campo es obligatorio y debe tener entre 8 y 20 caracteres.
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Nueva contraseña del usuario. Este campo es obligatorio y debe tener entre 8 y 20 caracteres.
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmación de la nueva contraseña. Este campo es obligatorio y debe tener entre 8 y 20 caracteres.
        /// </summary>
        /// <remarks>
        /// Este campo debe coincidir con el campo de NewPassord
        /// </remarks>
        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}