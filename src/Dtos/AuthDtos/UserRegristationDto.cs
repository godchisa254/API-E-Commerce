using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos
{
    public class UserRegristationDto
    {
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
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
        
    }
}