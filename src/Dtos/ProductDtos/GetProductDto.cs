
namespace taller1.src.Dtos.ProductDtos
{
    /// <summary>
    /// DTO utilizado para obtener un producto y la URL de la imagen.
    /// </summary>
    public class GetProductDto
    {
        /// <summary>
        /// Identificador del tipo de producto al que pertenece.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="ProductType"/>.
        /// </remarks>
        public int ProductTypeID { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <remarks>
        /// Este campo no es obligatorio a diferencia del modelo <see cref="Product"/>.
        /// </remarks>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// Cantidad de unidades disponibles en stock.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// URL de la imagen asociada al producto.
        /// </summary>
        public string Image { get; set; } = string.Empty;
    }
}