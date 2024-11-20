 
using taller1.src.Dtos;
using taller1.src.Helpers;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<Receipt>> GetAll(QueryReceipt query); 
    }
}