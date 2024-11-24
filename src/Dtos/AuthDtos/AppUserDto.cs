using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.AuthDtos
{
    /// <summary>
    /// Representa un Data Transfer Object (DTO) para un usuario en la aplicación.
    /// </summary>
    public class AppUserDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
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

        /// <summary>
        /// Indica si el usuario está habilitado en el sistema. Por defecto, está habilitado.
        /// </summary>
        public bool enabledUser { get; set; } = true;

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        public DateOnly Birthdate { get; set; }

        /// <summary>
        /// Género del usuario. El valor debe estar en el rango de 0 a 3, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decirlo. 
        /// </summary>
        [Range(0, 4)]
        public int Gender { get; set; }




    }
}