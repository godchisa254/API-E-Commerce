using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa una boleta generado por una compra realizada por un usuario.
    /// </summary>
    public class Receipt
    {
        /// <summary>
        /// Identificador único de la boleta.
        /// </summary>
        /// <remarks>
        /// Es generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Identificador único del usuario asociado a la boleta.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="AppUser"/>.
        /// </remarks>
        [ForeignKey("AppUser")]
        [Required]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// País asociado a la dirección de la boleta.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Ciudad asociada a la dirección de la boleta.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Comuna asociada a la dirección de la boleta.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Commune { get; set; } = string.Empty;

        /// <summary>
        /// Calle asociada a la dirección de la boleta.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Fecha en que se generó la boleta.
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Monto total de la compra.
        /// </summary>
        /// <remarks>
        /// Este campo se almacena con un formato decimal (9, 2).
        /// </remarks>
        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public float Total { get; set; }

        /// <summary>
        /// Usuario asociado al recibo.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="AppUser"/>.
        /// </remarks>
        [Required]
        public AppUser User { get; set; } = null!;

        /// <summary>
        /// Detalles de los productos asociados a la boleta.
        /// </summary>
        /// <remarks>
        /// Relación de uno a muchos con la entidad <see cref="ReceiptItemDetail"/>.
        /// </remarks>
        [Required]
        public List<ReceiptItemDetail> ReceiptItemDetails { get; set; } = new List<ReceiptItemDetail>();
    }
}
