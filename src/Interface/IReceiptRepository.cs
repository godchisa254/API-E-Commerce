using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Models;

namespace taller1.src.Interface
{
    /// <summary>
    /// Interfaz que define los métodos necesarios para la gestión de recibos en el sistema.
    /// </summary>
    public interface IReceiptRepository
    {
        /// <summary>
        /// Crea un nuevo recibo basado en la información proporcionada en el DTO de solicitud de recibo y el carrito de compras.
        /// </summary>
        /// <param name="receiptRequestDto">El objeto <see cref="CreateReceiptRequestDto"/> que contiene los detalles del recibo a crear.</param>
        /// <param name="shoppingCart">El objeto <see cref="ShoppingCart"/> que contiene los artículos comprados por el usuario.</param>
        /// <returns>Devuelve el objeto <see cref="Receipt"/> creado con los detalles del recibo y la información del carrito de compras.</returns>
        Task<Receipt> CreateReceipt(CreateReceiptRequestDto receiptRequestDto, ShoppingCart shoppingCart);

        /// <summary>
        /// Obtiene todos los recibos del sistema aplicando filtros y paginación.
        /// </summary>
        /// <param name="query">El objeto <see cref="QueryReceipt"/> que contiene los filtros, número de página y tamaño de página para la consulta.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Receipt"/> que representan los recibos almacenados en el sistema.</returns>
        Task<List<Receipt>> GetAll(QueryReceipt query);

        /// <summary>
        /// Obtiene todos los recibos asociados a un usuario específico, identificado por su ID de usuario.
        /// </summary>
        /// <param name="userId">El identificador único del usuario cuyos recibos se desean obtener.</param>
        /// <returns>Devuelve una lista de objetos <see cref="Receipt"/> asociados al usuario especificado.</returns>
        Task<List<Receipt>> GetByUserId(string userId);
    }
}
