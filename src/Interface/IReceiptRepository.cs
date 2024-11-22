using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IReceiptRepository
    {
        Task<Receipt> CreateReceipt(CreateReceiptRequestDto receiptRequestDto, ShoppingCart shoppingCart, AppUser user);
        Task<List<Receipt>> GetAll(QueryReceipt query);
    }
}