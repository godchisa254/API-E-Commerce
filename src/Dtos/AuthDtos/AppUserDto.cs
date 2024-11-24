using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.AuthDtos
{
    public class AppUserDto
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        public bool enabledUser { get; set; } = true;

        public DateOnly Birthdate { get; set; }

        [Range(0, 4)]
        public int Gender { get; set; }




    }
}