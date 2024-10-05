using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("AppUser")]
        [Required]
        public string UserID { get; set; } = string.Empty;

        public AppUser AppUser { get; set; } = null!;
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = [];
    }
}
