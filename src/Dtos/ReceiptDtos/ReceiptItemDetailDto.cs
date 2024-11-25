namespace taller1.src.Dtos.ReceiptDtos
{
    /// <summary>
    /// DTO utilizado para obtener los productos de una boleta.
    /// </summary>
    public class ReceiptItemDetailDto
    { 

        /// <summary>
        /// Identificador único del producto asociado a la boleta.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="Product"/>.
        /// </remarks>
        public int ProductID { get; set; } 

        /// <summary>
        /// Nombre del producto en el detalle.
        /// </summary>
        public string Name { get; set; } = string.Empty; 

        /// <summary>
        /// Tipo o categoría del producto en el detalle.
        /// </summary>
        public string Type { get; set; } = string.Empty; 

        /// <summary>
        /// Precio unitario del producto en el detalle.
        /// </summary>
        public float UnitPrice { get; set; } 

        /// <summary>
        /// Cantidad del producto en el detalle.
        /// </summary>
        public int Quantity { get; set; } 

        /// <summary>
        /// Precio total calculado del producto en el detalle.
        /// </summary>
        public float TotalItemPrice { get; set; } 

        /// <summary>
        /// URL de la imagen asociada al producto.
        /// </summary>
        public string Image { get; set; } = string.Empty;
    }
}