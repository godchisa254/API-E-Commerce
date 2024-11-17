
using taller1.src.Interface;
using Microsoft.EntityFrameworkCore;
using taller1.src.Models;
using taller1.src.Data;

namespace taller1.src.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDBContext _context;

        public ShoppingCartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

    }

}