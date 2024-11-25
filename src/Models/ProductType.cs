using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa un tipo o categoría de producto.
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// Identificador único del tipo de producto.
        /// </summary>
        /// <remarks>
        /// Es generado automáticamente por la base de datos.
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Nombre o descripción del tipo de producto.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y tiene un tamaño máximo de 255 caracteres.
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Propiedad de navegación de la lista de productos asociados a este tipo de producto.
        /// </summary>
        /// <remarks>
        /// Relación de uno a muchos con la entidad <see cref="Product"/>.
        /// </remarks>
        public List<Product> Products { get; set; } = [];
    }
}
