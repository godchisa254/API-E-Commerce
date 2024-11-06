using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Interface
{
    public interface IProductRepository
    {
        Task <List<Product>> GetAll(QueryObject query);
        Task<Product?> GetById(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product?> UpdateProduct(int id, UpdateProductRequestDto productDto);
        Task<Product?> DeleteProduct(int id);
    }
}