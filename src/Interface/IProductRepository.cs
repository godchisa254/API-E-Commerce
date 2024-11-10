using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Helpers;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IProductRepository
    {
        Task<List<GetProductDto>> GetAll(QueryObject query);
        Task<GetProductDto?> GetById(int id);
        Task<GetProductDto> CreateProduct(Product productModel);
        Task<GetProductDto?> UpdateProduct(int id, UpdateProductRequestDto productDto);
        Task<GetProductDto?> DeleteProduct(int id);
        Task<bool> ProductExists(string name, int productTypeId);
    }
}