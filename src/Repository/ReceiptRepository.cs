using Microsoft.EntityFrameworkCore;
using taller1.src.Data;
using taller1.src.Dtos;
using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Repository
{
    public class ReceiptRepository : IReceiptRepository
    { 
        private readonly ApplicationDBContext _context; 

        public ReceiptRepository(ApplicationDBContext context)
        {
            _context = context; 
        }
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

        public async Task<Receipt> CreateReceipt(CreateReceiptRequestDto receiptRequestDto, ShoppingCart shoppingCart)
        {
            var receiptModel = receiptRequestDto.ToReceiptModel(shoppingCart);
            await _context.Receipts.AddAsync(receiptModel);
            await _context.SaveChangesAsync();
            return receiptModel;
        }

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