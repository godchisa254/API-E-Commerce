using taller1.src.Dtos.ProductDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    /// <summary>
    /// Proporciona métodos para mapear entre la entidad <see cref="Product"/> y sus DTOs.
    /// </summary>
    public static class ProductMapper
    {
        /// <summary>
        /// Convierte un modelo de producto en un DTO para la respuesta.
        /// </summary>
        /// <param name="productModel">El modelo de producto a convertir.</param>
        /// <returns>
        /// Un objeto <see cref="GetProductDto"/> que representa el producto.
        /// </returns>
        public static GetProductDto ToGetProductDto(this Product productModel)
        {
            return new GetProductDto
            {
                ID = productModel.ID,
                ProductTypeID = productModel.ProductTypeID,
                Name = productModel.Name,
                Price = productModel.Price,
                Stock = productModel.Stock,
                Image = productModel.Image
            };
        }

        /// <summary>
        /// Convierte un DTO de creación de producto en un modelo de producto.
        /// </summary>
        /// <param name="createProductRequestDto">El DTO de creación de producto.</param>
        /// <param name="imageUrl">La URL de la imagen del producto.</param>
        /// <returns>
        /// Un objeto <see cref="Product"/> que representa el producto a crear.
        /// </returns>
        public static Product ToProduct(this CreateProductRequestDto createProductRequestDto, string imageUrl)
        {
            return new Product
            {
                ProductTypeID = createProductRequestDto.ProductTypeID,
                Name = createProductRequestDto.Name,
                Price = createProductRequestDto.Price,
                Stock = createProductRequestDto.Stock,
                Image = imageUrl
            };
        }

        /// <summary>
        /// Convierte un DTO de obtención de producto en un modelo de producto.
        /// </summary>
        /// <param name="getProductDto">El DTO de obtención de producto.</param>
        /// <returns>
        /// Un objeto <see cref="Product"/> que representa el producto.
        /// </returns>
        public static Product ToProductFromGetDto(this GetProductDto getProductDto)
        {
            return new Product
            {
                ID = getProductDto.ID,
                ProductTypeID = getProductDto.ProductTypeID,
                Name = getProductDto.Name,
                Price = getProductDto.Price,
                Stock = getProductDto.Stock,
                Image = getProductDto.Image
            };
        }
    }
}