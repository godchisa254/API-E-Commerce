using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("ShoppingCart")]
        [Required]
        public int ShoppingCartID { get; set; }

        [ForeignKey("Product")]
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ShoppingCart ShoppingCart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
