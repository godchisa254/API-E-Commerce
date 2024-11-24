using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa un producto en el sistema.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        /// <remarks>
        /// Es generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Identificador del tipo de producto al que pertenece.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="ProductType"/>.
        /// </remarks>
        [ForeignKey("ProductType")]
        [Required]
        public int ProductTypeID { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y tiene un tamaño máximo de 65 caracteres.
        /// </remarks>
        [Required]
        [StringLength(65)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y se almacena en la base de datos con un formato decimal (9, 2).
        /// </remarks>
        [Required]
        [Column(TypeName = "decimal(9, 2)")]
        public float Price { get; set; }

        /// <summary>
        /// Cantidad de unidades disponibles en stock.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio.
        /// </remarks>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// URL de la imagen asociada al producto.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y tiene un tamaño máximo de 255 caracteres.
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Propiedad de navegación de los productos que van en la boleta "Receipt".
        /// </summary>
        /// <remarks>
        /// Relación de uno a muchos con la entidad <see cref="ReceiptItemDetail"/>.
        /// </remarks>
        public List<ReceiptItemDetail> ReceiptItemDetails { get; set; } = [];

        /// <summary>
        /// Propiedad de navegación de elementos de carrito de compras que incluyen este producto.
        /// </summary>
        /// <remarks>
        /// Relación de uno a muchos con la entidad <see cref="ShoppingCartItem"/>.
        /// </remarks>
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = [];

        /// <summary>
        /// Propiedad de navegación del tipo de producto al que pertenece.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="ProductType"/>.
        /// Este campo no puede ser nulo.
        /// </remarks>
        public ProductType ProductType { get; set; } = null!;
    }
}
