using taller1.src.Data;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Models;
using Microsoft.EntityFrameworkCore;

namespace taller1.src.Repository
{
    /// <summary>
    /// Repositorio para los productos, gestionando operaciones como creación, modificación y consulta de productos.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        
        /// <summary>
        /// Constructor del repositorio de productos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los productos del sistema con la capacidad de aplicar filtros y paginación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryObject"/> que contiene los filtros, el número de página y el tamaño de página para la consulta.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Product"/> que representan los productos del sistema.</returns>
        public async Task<(List<Product>, int)> GetAll(QueryObject query)
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

            var totalCount = await products.CountAsync();
            var productModels = await products.Skip(skipNumber).Take(pageSize).ToListAsync();

            return (productModels, totalCount);
        }

        /// <summary>
        /// Obtiene un producto específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del producto.</param>
        /// <returns>Devuelve un objeto <see cref="Product"/> si el producto existe, de lo contrario <c>null</c>.</returns>
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

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="productModel">El objeto <see cref="Product"/> que contiene los detalles del nuevo producto.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> creado con los datos asignados, incluyendo el ID.</returns>
        public async Task<Product> CreateProduct(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        /// <summary>
        /// Actualiza un producto existente en el sistema con nuevos detalles.
        /// </summary>
        /// <param name="id">El identificador único del producto que se va a actualizar.</param>
        /// <param name="productDto">El objeto <see cref="UpdateProductRequestDto"/> que contiene los nuevos detalles del producto.</param>
        /// <param name="imageUrl">La URL de la imagen asociada al producto. Si no se cambia, puede ser <c>null</c>.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> actualizado si el producto fue encontrado y actualizado, de lo contrario <c>null</c>.</returns>
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

        /// <summary>
        /// Elimina un producto del sistema por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del producto a eliminar.</param>
        /// <returns>Devuelve el objeto <see cref="Product"/> que fue eliminado, o <c>null</c> si no se encontró el producto.</returns>
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

        /// <summary>
        /// Verifica si un producto con el mismo nombre y tipo de producto ya existe en el sistema.
        /// </summary>
        /// <param name="name">El nombre del producto a verificar.</param>
        /// <param name="productTypeId">El identificador del tipo de producto a verificar.</param>
        /// <returns>Devuelve <c>true</c> si el producto ya existe, de lo contrario <c>false</c>.</returns>
        public async Task<bool> ProductExists(string? name, int? productTypeId)
        {
            return await _context.Products
                .AnyAsync(p => p.Name == name && p.ProductTypeID == productTypeId);
        }

    }
}