using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Models;

namespace taller1.src.Dtos
{
    public class UserRegristationDto
    {
        [Required]
        [StringLength(20, ErrorMessage = "La contrasena no puede ser mayor a 20 caracteres")]
        [MinLength(8, ErrorMessage = "la contraseña debe tener al menos 8 caracteres")]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [CustomValidation(typeof(EditProfileUserDto), nameof(ValidateBirthdate))]
        public DateOnly Birthdate { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(0, 4)]
        public int Gender { get; set; } 

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string ConfirmPassword { get; set; } = string.Empty;



        public static ValidationResult? ValidateBirthdate(DateOnly birthdate, ValidationContext context)
    {
        if (birthdate.Year >= DateTime.Now.Year)
        {
            return new ValidationResult("La fecha de nacimiento debe ser menor al año actual.");
        }
        return ValidationResult.Success;
    }
        
    }
}