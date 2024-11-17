using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;

namespace taller1.src.Interface
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetCartByUserId(string userId);
        Task SaveCart(ShoppingCart cart, string userId);
        Task<Product> GetProductById(int productId);
    }

}