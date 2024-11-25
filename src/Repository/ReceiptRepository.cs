using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Repository
{
    /// <summary>
    /// Repositorio para las boletas, gestionando operaciones como creación, modificación y consulta de boletas.
    /// </summary>
    public class ReceiptRepository : IReceiptRepository
    { 
        private readonly ApplicationDBContext _context; 

        /// <summary>
        /// Constructor del repositorio de autenticación.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public ReceiptRepository(ApplicationDBContext context)
        {
            _context = context; 
        }

        /// <summary>
        /// Obtiene todos los recibos del sistema aplicando filtros y paginación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryReceipt"/> que contiene los filtros, número de página y tamaño de página para la consulta.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Receipt"/> que representan los recibos almacenados en el sistema.</returns>
        public async Task<List<Receipt>> GetAll(QueryReceipt query)
        {
            var pageNumber = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var receipts = _context.Receipts.AsQueryable();
            receipts = receipts
                .Include(receipt => receipt.ReceiptItemDetails) 
                .ThenInclude(item => item.Product);

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var normalizedQueryName = query.Name.Trim().ToLower();
                receipts = receipts.Where(r => r.User.Name.ToLower().Contains(normalizedQueryName));
            } 

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var propertyInfo = typeof(Receipt).GetProperty(query.SortBy);
                if (propertyInfo != null)
                {
                    receipts = query.IsDescending ? receipts.OrderByDescending(r => EF.Property<object>(r, query.SortBy)) : receipts.OrderBy(r => EF.Property<object>(r, query.SortBy));
                }
                else
                {
                    throw new ArgumentException($"Invalid sort property: {query.SortBy}, Use one of the following: {string.Join(", ", propertyInfo)}");
                }
            }

            var skipNumber = (pageNumber - 1) * pageSize;
            var receiptsModels = await receipts.Skip(skipNumber).Take(pageSize).ToListAsync();

            return receiptsModels;
        }

        /// <summary>
        /// Crea un nuevo recibo basado en la información proporcionada en el DTO de solicitud de recibo y el carrito de compras.
        /// </summary>
        /// <param name="receiptRequestDto">El objeto <see cref="CreateReceiptRequestDto"/> que contiene los detalles del recibo a crear.</param>
        /// <param name="shoppingCart">El objeto <see cref="ShoppingCart"/> que contiene los artículos comprados por el usuario.</param>
        /// <returns>Devuelve el objeto <see cref="Receipt"/> creado con los detalles del recibo y la información del carrito de compras.</returns>
        public async Task<Receipt> CreateReceipt(CreateReceiptRequestDto receiptRequestDto, ShoppingCart shoppingCart)
        {
            var receiptModel = receiptRequestDto.ToReceiptModel(shoppingCart);
            await _context.Receipts.AddAsync(receiptModel);
            await _context.SaveChangesAsync();
            return receiptModel;
        }

        /// <summary>
        /// Obtiene todos los recibos asociados a un usuario específico, identificado por su ID de usuario.
        /// </summary>
        /// <param name="userId">El identificador único del usuario cuyos recibos se desean obtener.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Receipt"/> asociados al usuario especificado.</returns>
        public async Task<List<Receipt>> GetByUserId(string userId)
        {
            return await _context.Receipts
                .Include(receipt => receipt.ReceiptItemDetails)  
                .ThenInclude(item => item.Product)  
                .Where(receipt => receipt.UserID == userId)
                .ToListAsync();
        }
    }
}