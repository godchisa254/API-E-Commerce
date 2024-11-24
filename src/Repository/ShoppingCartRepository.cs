
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
        public async Task<ShoppingCart> GetCartByUserId(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(cart => cart.AppUser)  
                .Include(cart => cart.ShoppingCartItems)
                .ThenInclude(item => item.Product)
                .ThenInclude(product => product.ProductType)
                .FirstOrDefaultAsync(cart => cart.UserID == userId);
 
            if (cart == null)
            {
                cart = new ShoppingCart { UserID = userId, ShoppingCartItems = new List<ShoppingCartItem>() };
            }
            return cart;
        }
        public async Task SaveCart(ShoppingCart cart, string userId)
        {
            var existingCart = await _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (existingCart == null)
            {
                _context.ShoppingCarts.Add(cart);
            }
            else
            {
                existingCart.ShoppingCartItems = cart.ShoppingCartItems;
            }

            await _context.SaveChangesAsync();
        }

    }
}