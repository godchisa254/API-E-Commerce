using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa un producto dentro de un carrito de compras.
    /// </summary>
    public class ShoppingCartItem
    {
        /// <summary>
        /// Identificador único del producto del carrito de compras.
        /// </summary>
        /// <remarks>
        /// Generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Identificador único del carrito de compras al que pertenece el producto.
        /// </summary>
        /// <remarks>
        /// Clave foránea que referencia a la entidad <see cref="ShoppingCart"/>.
        /// </remarks>
        [ForeignKey("ShoppingCart")]
        [Required]
        public int ShoppingCartID { get; set; }

        /// <summary>
        /// Identificador único del producto contenido en el producto.
        /// </summary>
        /// <remarks>
        /// Clave foránea que referencia a la entidad <see cref="Product"/>.
        /// </remarks>
        [ForeignKey("Product")]
        [Required]
        public int ProductID { get; set; }

        /// <summary>
        /// Cantidad del producto contenida en el producto.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Carrito de compras al que pertenece el producto.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="ShoppingCart"/>.
        /// </remarks>
        public ShoppingCart ShoppingCart { get; set; } = null!;

        /// <summary>
        /// Producto asociado al producto.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="Product"/>.
        /// </remarks>
        public Product Product { get; set; } = null!;
    }
}
