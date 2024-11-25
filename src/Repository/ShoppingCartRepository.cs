using taller1.src.Interface;
using Microsoft.EntityFrameworkCore;
using taller1.src.Models;
using taller1.src.Data;

namespace taller1.src.Repository
{
    /// <summary>
    /// Repositorio para el carrito de compras, gestionando operaciones como guardado y consulta de carritos.
    /// </summary>
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDBContext _context;

        /// <summary>
        /// Constructor del repositorio del carrito de compras.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public ShoppingCartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene el carrito de compras de un usuario específico.
        /// </summary>
        /// <param name="userId">El ID del usuario para el cual se desea obtener el carrito de compras.</param>
        /// <returns>Devuelve el objeto <see cref="ShoppingCart"/> asociado al usuario, o null si no existe.</returns>
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

        /// <summary>
        /// Guarda el carrito de compras de un usuario.
        /// </summary>
        /// <param name="cart">El objeto <see cref="ShoppingCart"/> que contiene los datos del carrito que se desea guardar.</param>
        /// <param name="userId">El ID del usuario cuyo carrito debe ser guardado.</param>
        /// <returns>Una tarea que representa el proceso asincrónico de guardar el carrito.</returns>
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