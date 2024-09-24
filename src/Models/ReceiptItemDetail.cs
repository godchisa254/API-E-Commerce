using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    public class ReceiptItemDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [ForeignKey("Receipt")]
        [Required]
        public int ReceiptId { get; set; }

        [ForeignKey("Product")]
        [Required]
        public int ProductID { get; set; }

        [Required]
        [StringLength(65)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal TotalPrice { get; set; }
        
        public Receipt Receipt { get; set; } = null!;
        public Product Product { get; set; } = null!;

    }
}
