using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos.Product;
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
                ID = productModel.ID,
                ProductTypeID = productModel.ProductTypeID,
                Name = productModel.Name,
                Price = productModel.Price,
                Stock = productModel.Stock,
                Image = productModel.Image
            };
        }
    }
}
