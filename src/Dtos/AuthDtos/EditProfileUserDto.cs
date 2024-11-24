using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;

namespace taller1.src.Dtos.AuthDtos
{
    public class EditProfileUserDto
    {
        [StringLength(255)]
        public string? Name { get; set; } = string.Empty;

        public DateOnly? Birthdate { get; set; }

        [Range(0, 4)]
        public int? Gender { get; set; }


    }
}