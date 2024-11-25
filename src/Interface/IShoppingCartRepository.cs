using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para gestionar los carritos de compras de los usuarios.
    /// </summary>
    public interface IShoppingCartRepository
    {
        /// <summary>
        /// Obtiene el carrito de compras de un usuario específico.
        /// </summary>
        /// <param name="userId">El ID del usuario para el cual se desea obtener el carrito de compras.</param>
        /// <returns>Devuelve el objeto <see cref="ShoppingCart"/> asociado al usuario, o null si no existe.</returns>
        Task<ShoppingCart> GetCartByUserId(string userId);

        /// <summary>
        /// Guarda el carrito de compras de un usuario.
        /// </summary>
        /// <param name="cart">El objeto <see cref="ShoppingCart"/> que contiene los datos del carrito que se desea guardar.</param>
        /// <param name="userId">El ID del usuario cuyo carrito debe ser guardado.</param>
        /// <returns>Una tarea que representa el proceso asincrónico de guardar el carrito.</returns>
        Task SaveCart(ShoppingCart cart, string userId);
    }
}
