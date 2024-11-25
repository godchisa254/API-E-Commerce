using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa el carrito de compras asociado a un usuario.
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// Identificador único del carrito de compras.
        /// </summary>
        /// <remarks>
        /// Generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Identificador único del usuario propietario del carrito de compras.
        /// </summary>
        /// <remarks>
        /// Clave foránea que referencia a la entidad <see cref="AppUser"/>.
        /// </remarks>
        [ForeignKey("AppUser")]
        [Required]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// Usuario asociado al carrito de compras.
        /// </summary>
        /// <remarks>
        /// Relación con la entidad <see cref="AppUser"/>.
        /// </remarks>
        public AppUser AppUser { get; set; } = null!;

        /// <summary>
        /// Lista de producto contenidos en el carrito de compras.
        /// </summary>
        /// <remarks>
        /// Relación de uno a muchos con la entidad <see cref="ShoppingCartItem"/>.
        /// </remarks>
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = [];
    }
}
