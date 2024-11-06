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

        public async Task<List<Product>> GetAll(QueryObject query)
        {
            var pageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var products = _context.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(x => x.Name.Contains(query.Name));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var propertyInfo = typeof(Product).GetProperty(query.SortBy);
                if (propertyInfo != null)
                {
                    products = query.IsDescending ? products.OrderByDescending(x => EF.Property<object>(x, query.SortBy)) : products.OrderBy(x => EF.Property<object>(x, query.SortBy));
                }
                else
                {
                    throw new ArgumentException($"Invalid sort property: {query.SortBy}");
                }
            }

            var skipNumber = (pageNumber - 1) * pageSize;
            return await products.Skip(skipNumber).Take(pageSize).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateProduct(int id, UpdateProductRequestDto productDto)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }

            productModel.ProductTypeID = productDto.ProductTypeID ?? productModel.ProductTypeID;
            productModel.Name = productDto.Name ?? productModel.Name;
            productModel.Price = productDto.Price ?? productModel.Price;
            productModel.Stock = productDto.Stock ?? productModel.Stock;
            productModel.Image = productDto.Image ?? productModel.Image;

            await _context.SaveChangesAsync();
            return productModel;
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