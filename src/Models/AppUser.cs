using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace taller1.src.Models
{
    public class AppUser : IdentityUser
    {
        // Removemos el ID personalizado ya que IdentityUser ya proporciona Id como string
        
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [Range(0, 4)]
        public int Gender { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        public List<Receipt> Receipts { get; set; } = [];
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}