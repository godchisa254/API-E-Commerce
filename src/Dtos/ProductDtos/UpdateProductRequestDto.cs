namespace taller1.src.Dtos.ProductDtos
{
    /// <summary>
    /// DTO utilizado para actualizar un producto existente.
    /// </summary>
    public class UpdateProductRequestDto
    {
        /// <summary>
        /// Identificador del tipo de producto.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y permite modificar la categoría o tipo al que pertenece el producto.
        /// </remarks>
        public int? ProductTypeID { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y se utiliza para actualizar el nombre del producto.
        /// </remarks>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Precio del producto.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y permite modificar el precio del producto.
        /// </remarks>
        public float? Price { get; set; }

        /// <summary>
        /// Cantidad de unidades en stock.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y se utiliza para actualizar la cantidad disponible en inventario.
        /// </remarks>
        public int? Stock { get; set; }

        /// <summary>
        /// Archivo de imagen del producto.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y permite actualizar la imagen asociada al producto.
        /// Se espera un archivo enviado como parte de la solicitud.
        /// </remarks>
        public IFormFile? Image { get; set; }
    }
}
