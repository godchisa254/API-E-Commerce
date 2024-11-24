using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    public class Receipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("AppUser")]
        [Required]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Commune { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Street { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public float Total { get; set; }

        [Required]
        public AppUser User { get; set; } = null!;
        
        [Required]
        public List<ReceiptItemDetail> ReceiptItemDetails { get; set; } = new List<ReceiptItemDetail>();
    }
}
