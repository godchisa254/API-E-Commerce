using taller1.src.Helpers;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para la intervención en la base de datos para los productos.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene todos los productos del sistema con la capacidad de aplicar filtros y paginación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryObject"/> que contiene los filtros, el número de página y el tamaño de página para la consulta.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Product"/> que representan los productos del sistema.</returns>
        Task<(List<Product>, int)> GetAll(QueryObject query);

        /// <summary>
        /// Obtiene un producto específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del producto.</param>
        /// <returns>Devuelve un objeto <see cref="Product"/> si el producto existe, de lo contrario <c>null</c>.</returns>
        Task<Product?> GetById(int id);

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="productModel">El objeto <see cref="Product"/> que contiene los detalles del nuevo producto.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> creado con los datos asignados, incluyendo el ID.</returns>
        Task<Product> CreateProduct(Product productModel);

        /// <summary>
        /// Actualiza un producto existente en el sistema con nuevos detalles.
        /// </summary>
        /// <param name="id">El identificador único del producto que se va a actualizar.</param>
        /// <param name="productDto">El objeto <see cref="UpdateProductRequestDto"/> que contiene los nuevos detalles del producto.</param>
        /// <param name="imageUrl">La URL de la imagen asociada al producto. Si no se cambia, puede ser <c>null</c>.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> actualizado si el producto fue encontrado y actualizado, de lo contrario <c>null</c>.</returns>
        Task<Product?> UpdateProduct(int id, UpdateProductRequestDto productDto, string? imageUrl);

        /// <summary>
        /// Elimina un producto del sistema por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del producto a eliminar.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> que fue eliminado, o <c>null</c> si no se encontró el producto.</returns>
        Task<Product?> DeleteProduct(int id);

        /// <summary>
        /// Verifica si un producto con el mismo nombre y tipo de producto ya existe en el sistema.
        /// </summary>
        /// <param name="name">El nombre del producto a verificar.</param>
        /// <param name="productTypeId">El identificador del tipo de producto a verificar.</param>
        /// <returns>Devuelve <c>true</c> si el producto ya existe, de lo contrario <c>false</c>.</returns>
        Task<bool> ProductExists(string? name, int? productTypeId);
        Task<List<ProductType>> GetProductTypes();
    }
}
