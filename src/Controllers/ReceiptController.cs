using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    { 
        private const string CartCookieKey = "TicketsList"; 
        private const string UserCookieKey = "UserGUID";
        private readonly IReceiptRepository _receiptRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAuthRepository _authRepository;
        public ReceiptController(IReceiptRepository receiptRepository, IShoppingCartRepository shoppingCartRepository, IAuthRepository authRepository)
        {
            _receiptRepository = receiptRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _authRepository = authRepository;
        }

        [HttpGet("Obtener_todas_las_boletas")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] QueryReceipt query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var receipts = await _receiptRepository.GetAll(query);
            var receiptsDto = receipts.Select(receipt => receipt.ToGetReceiptDto()).ToList();
            return Ok(receiptsDto);
        }

        [HttpGet("Obtener_boletas_propias")]
        [Authorize]
        public async Task<IActionResult> GetReceiptById()
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            var receipts = await _receiptRepository.GetByUserId(userId);
            if (receipts == null)
            {
                return NotFound();
            }

            var receiptDtos = receipts.Select(r => r.ToGetReceiptDto()).ToList();
            return Ok(receiptDtos);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePurchase(CreateReceiptRequestDto receiptRequestDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!;
            var userId = userIdClaim.Value;

            try
            {
                var shoppingCart = await _shoppingCartRepository.GetCartByUserId(userId);

                if (shoppingCart == null || !shoppingCart.ShoppingCartItems.Any())
                {
                    return BadRequest("Shopping cart is empty or does not exist.");
                }

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

                
                var user = await _authRepository.GetUserByid(userId); 
                if (user == null){ return BadRequest("User not found, you must be logged in to make a purchase."); }
                var receipt = await _receiptRepository.CreateReceipt(receiptRequestDto, shoppingCart, user);
                var receiptDto = receipt.ToGetReceiptDto();
                // await _shoppingCartRepository.ClearCart(shoppingCart); //TODO: Implementar método para limpiar carrito
                // await _productRepository.UpdateStock(shoppingCart.ShoppingCartItems); //TODO: Implementar método para actualizar stock

                return Ok(receiptDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
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
            if (userId == null)
            {
                return null;
            }
            return userId;
        }
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