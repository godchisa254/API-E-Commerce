using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers; 

namespace taller1.src.Controllers
{
    /// <summary>
    /// Controlador para manejar las solicitudes relacionadas con las boletas (recibos) de compras.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    { 
        private const string UserCookieKey = "UserGUID";
        private readonly IReceiptRepository _receiptRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository; 

        /// <summary>
        /// Constructor del controlador de boletas, que inyecta las dependencias necesarias.
        /// </summary>
        /// <param name="receiptRepository">Repositorio para manejar las boletas.</param>
        /// <param name="shoppingCartRepository">Repositorio para manejar los carritos de compras.</param> 
        public ReceiptController(IReceiptRepository receiptRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _receiptRepository = receiptRepository;
            _shoppingCartRepository = shoppingCartRepository; 
        }

        /// <summary>
        /// Obtiene todas las boletas. Solo accesible para administradores.
        /// </summary>
        /// <param name="query">Parámetros de consulta para filtrar, ordenar y paginar las boletas.</param>
        /// <returns>Una lista de todas las boletas que cumplen con los criterios establecidos en la consulta.</returns>
        [HttpGet("Obtener_todas_las_boletas")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] QueryReceipt query)
        {
            // Verifica la validez del modelo
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtiene todas las boletas según la consulta
            var receipts = await _receiptRepository.GetAll(query);
            var receiptsDto = receipts.Select(receipt => receipt.ToGetReceiptDto()).ToList();
            return Ok(receiptsDto);
        }

        /// <summary>
        /// Obtiene las boletas del usuario autenticado.
        /// </summary>
        /// <returns>Una lista de boletas del usuario autenticado, o un error si no se encuentran.</returns>
        [HttpGet("Obtener_boletas_propias")]
        [Authorize]
        public async Task<IActionResult> GetReceiptById()
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Obtiene las boletas asociadas al usuario autenticado
            var receipts = await _receiptRepository.GetByUserId(userId);
            if (receipts == null)
            {
                return NotFound();
            }

            var receiptDtos = receipts.Select(r => r.ToGetReceiptDto()).ToList();
            return Ok(receiptDtos);
        }

        /// <summary>
        /// Crea una nueva compra (boleta) para el usuario autenticado.
        /// </summary>
        /// <param name="receiptRequestDto">Objeto con la información de la boleta que se está creando.</param>
        /// <returns>Una boleta con la información de la compra realizada, o un error si ocurre algún problema.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePurchase(CreateReceiptRequestDto receiptRequestDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;
            var userId = userIdClaim.Value;

            try
            {
                // Obtiene el carrito de compras del usuario
                var shoppingCart = await _shoppingCartRepository.GetCartByUserId(userId);

                if (shoppingCart == null || !shoppingCart.ShoppingCartItems.Any())
                {
                    return BadRequest("Shopping cart is empty or does not exist.");
                }

                // Verifica si hay productos con stock insuficiente
                var insufficientStockItems = shoppingCart.ShoppingCartItems
                    .Where(item => item.Quantity > item.Product.Stock)
                    .Where(item => item.Product.Stock > 0)
                    .ToList();

                if (insufficientStockItems.Any())
                {
                    var errorMessage = string.Join(", ", insufficientStockItems
                        .Select(item => $"Not enough stock of {item.Product.Name}"));

                    return BadRequest(errorMessage);
                }

                // Crea la boleta
                var receipt = await _receiptRepository.CreateReceipt(receiptRequestDto, shoppingCart);
                var receiptDto = receipt.ToGetReceiptDto();
                
                // TODO: Limpiar carrito y actualizar stock (pendiente de implementación)
                // await _shoppingCartRepository.ClearCart(shoppingCart); 
                // await _productRepository.UpdateStock(shoppingCart.ShoppingCartItems);

                return Ok(receiptDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado o crea un nuevo GUID si no está autenticado.
        /// </summary>
        /// <returns>El ID del usuario o un nuevo GUID si el usuario no está autenticado.</returns>
        private string? GetUserId()
        {
            string? userId;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                userId = GetOrCreateUserGuid();
            }
            return userId;
        }

        /// <summary>
        /// Obtiene o crea un nuevo GUID de usuario para usuarios no autenticados.
        /// </summary>
        /// <returns>El GUID del usuario.</returns>
        private string GetOrCreateUserGuid()
        {
            var userGuid = Request.Cookies[UserCookieKey];

            if (string.IsNullOrEmpty(userGuid))
            {
                userGuid = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    HttpOnly = false,
                    Secure = false,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append(UserCookieKey, userGuid, cookieOptions);
            }

            return userGuid;
        }
    }
}
