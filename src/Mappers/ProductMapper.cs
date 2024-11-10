using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    public static class ProductMapper
    {
        // Mapeo de productModel a GetProductDto
        public static GetProductDto ToGetProductDto(this Product productModel)
        {
            return new GetProductDto
            {
                ProductTypeID = productModel.ProductTypeID,
                Name = productModel.Name,
                Price = productModel.Price,
                Stock = productModel.Stock,
                Image = productModel.Image
            };
        }

        // Mapeo de CreateProductRequestDto a productModel
        public static Product ToProduct(this CreateProductRequestDto productDto, string imageUrl)
        {
            return new Product
            {
                ProductTypeID = productDto.ProductTypeID,
                Name = productDto.Name,
                Price = productDto.Price,
                Stock = productDto.Stock,
                Image = imageUrl
            };
        }
    }
}
