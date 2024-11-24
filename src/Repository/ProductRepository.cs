using taller1.src.Data;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
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
                var normalizedQueryName = query.Name.Trim().ToLower();
                products = products.Where(x => x.Name.ToLower().Contains(normalizedQueryName));
            }
            
            if (query.ProductTypeID.HasValue)
            {
                products = products.Where(p => p.ProductTypeID == query.ProductTypeID.Value);
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
                    throw new ArgumentException($"Invalid sort property: {query.SortBy}, use one of the following: ProductTypeID, Name, Price, Stock");
                }
            }

            var skipNumber = (pageNumber - 1) * pageSize;

            var productModels = await products.Skip(skipNumber).Take(pageSize).ToListAsync();

            return productModels;
        }

        public async Task<Product?> GetById(int id)
        {
            var productModel = await _context.Products
                .Include(p => p.ProductType).FirstOrDefaultAsync(p => p.ID == id);
            if (productModel == null)
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            return productModel;
        }

        public async Task<Product> CreateProduct(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> UpdateProduct(int id, UpdateProductRequestDto productDto, string? imageUrl)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }

            productModel.ProductTypeID = productDto.ProductTypeID ?? productModel.ProductTypeID;
            productModel.Name = productDto.Name ?? productModel.Name;
            productModel.Price = productDto.Price ?? productModel.Price;
            productModel.Stock = productDto.Stock ?? productModel.Stock;
            productModel.Image = imageUrl ?? productModel.Image;
            
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> DeleteProduct(int id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ID == id);
            if (productModel == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
        public async Task<bool> ProductExists(string? name, int? productTypeId)
        {
            return await _context.Products
                .AnyAsync(p => p.Name == name && p.ProductTypeID == productTypeId);
        }

    }
}