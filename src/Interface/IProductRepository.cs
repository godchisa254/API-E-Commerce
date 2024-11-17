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
        Task<List<Product>> GetAll(QueryObject query);
        Task<Product?> GetById(int id);
        Task<Product> CreateProduct(Product productModel);
        Task<Product?> UpdateProduct(int id, UpdateProductRequestDto productDto, string? imageUrl);
        Task<Product?> DeleteProduct(int id);
        Task<bool> ProductExists(string? name, int? productTypeId);
    }
}