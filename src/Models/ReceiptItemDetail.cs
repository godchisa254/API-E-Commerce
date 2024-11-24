using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa el detalle de un producto en una boleta.
    /// </summary>
    public class ReceiptItemDetail
    {
        /// <summary>
        /// Identificador único del detalle de la boleta.
        /// </summary>
        /// <remarks>
        /// Es generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Identificador único de la boleta asociada al detalle.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="Receipt"/>.
        /// </remarks>
        [ForeignKey("Receipt")]
        [Required]
        public int ReceiptId { get; set; }

        /// <summary>
        /// Identificador único del producto asociado a la boleta.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="Product"/>.
        /// </remarks>
        [ForeignKey("Product")]
        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// Nombre del producto en el detalle.
        /// </summary>
        [Required]
        [StringLength(65)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Tipo o categoría del producto en el detalle.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Precio unitario del producto en el detalle.
        /// </summary>
        /// <remarks>
        /// Este campo se almacena con un formato decimal (9, 2).
        /// </remarks>
        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public float UnitPrice { get; set; }

        /// <summary>
        /// Cantidad del producto en el detalle.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Precio total calculado del producto en el detalle.
        /// </summary>
        /// <remarks>
        /// Este campo se almacena con un formato decimal (9, 2).
        /// </remarks>
        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public float TotalPrice { get; set; }

        /// <summary>
        /// Boleta asociada al detalle.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="Receipt"/>.
        /// </remarks>
        public Receipt Receipt { get; set; } = null!;

        /// <summary>
        /// Producto asociado al detalle.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="Product"/>.
        /// </remarks>
        public Product Product { get; set; } = null!;
    }
}
