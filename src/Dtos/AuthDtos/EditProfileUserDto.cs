using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.AuthDtos
{
    public class EditProfileUserDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateOnly Birthdate { get; set; }

        [Required]
        [Range(0, 4)]
        public int Gender { get; set; }
    }
}