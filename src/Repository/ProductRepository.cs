using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Data;
using taller1.src.Dtos;
using taller1.src.Helpers;
using taller1.src.Interfaces;
using taller1.src.Models;
using Microsoft.EntityFrameworkCore;

namespace taller1.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<Product?> DeleteProduct(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
    }
}