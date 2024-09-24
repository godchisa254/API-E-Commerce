using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("ProductType")]
        [Required]
        public int ProductTypeID { get; set; }

        [Required]
        [StringLength(65)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [StringLength(255)]
        public string Image { get; set; } = string.Empty;

        public List<ReceiptItemDetail> ReceiptItemDetail { get; set; } = [];
        public List<ShoppingCartItem> ShoppingCartItem { get; set; } = [];
        public ProductType ProductType { get; set; } = null!;
    }
}
