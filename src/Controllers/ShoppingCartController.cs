using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    { 
        private const string CartCookieKey = "TicketsList"; 
        private const string UserCookieKey = "UserGUID";
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetCart()
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            { 
                var cart = await GetCart(userId);

                if (cart == null || !cart.ShoppingCartItems.Any())
                {
                    return NotFound("Shopping cart not found or is empty.");
                }
                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    item.Product.Name,
                    item.Product.Price,
                    item.Quantity,
                    TotalPrice = item.Product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        
        [HttpPost("add_product")]
        [AllowAnonymous]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            try
            { 
                var cart = await GetCart(userId);

                if (cart == null || !cart.ShoppingCartItems.Any())
                {
                    return NotFound("Shopping cart not found or is empty.");
                }

                var existingItem = cart.ShoppingCartItems
                    .FirstOrDefault(item => item.ProductID == productId);

                if (existingItem != null)
                {
                    if (existingItem.Product.Stock < existingItem.Quantity + quantity)
                    {
                        return BadRequest("Not enough stock available.");
                    }
                    existingItem.Quantity += quantity;
                }
                else
                { 
                    var product = await _shoppingCartRepository.GetProductById(productId); //TODO: arreglar el repositorio de productos para que devuelva un productModel y no DTO
                    if (product == null)
                    {
                        return NotFound("Product not found.");
                    }

                    cart.ShoppingCartItems.Add(new ShoppingCartItem
                    {
                        ProductID = productId,
                        Quantity = quantity,
                        Product = product
                    });
                    Console.WriteLine("Product added to cart");
                } 

                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    item.Product.Name,
                    item.Product.Price,
                    item.Quantity,
                    TotalPrice = item.Product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        
        [HttpPost("deduct_product")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveFromCart(int productId, int quantity)
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            
            try
            { 
                var cart = await GetCart(userId);

                if (cart == null || !cart.ShoppingCartItems.Any())
                { 
                    return NotFound("Shopping cart is empty.");
                }

                var existingItem = cart.ShoppingCartItems
                    .FirstOrDefault(item => item.ProductID == productId);

                if (existingItem == null)
                {
                    return NotFound("Product not found in the cart.");
                }
 
                existingItem.Quantity -= quantity;
                if (existingItem.Quantity <= 0)
                {
                    cart.ShoppingCartItems.Remove(existingItem);
                } 

                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    item.Product.Name,
                    item.Product.Price,
                    item.Quantity,
                    TotalPrice = item.Product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        } 

        [HttpDelete("remove_product")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var cart = await GetCart(userId);

                if (cart == null || !cart.ShoppingCartItems.Any())
                {
                    return NotFound("Shopping cart is empty.");
                }

                var existingItem = cart.ShoppingCartItems
                    .FirstOrDefault(item => item.ProductID == productId);

                if (existingItem == null)
                {
                    return NotFound("Product not found in the cart.");
                } 
                cart.ShoppingCartItems.Remove(existingItem);
 
                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    item.Product.Name,
                    item.Product.Price,
                    item.Quantity,
                    TotalPrice = item.Product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
 
        private void SaveCartAnyUser(ShoppingCart cart, string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }
            if (User.Identity?.IsAuthenticated ?? false)
            {
                _shoppingCartRepository.SaveCart(cart, userId);
            }
            else
            {
                SaveCartToCookies(userId, cart);
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
        
        private async Task<ShoppingCart> GetCart(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            if (User.Identity?.IsAuthenticated ?? false)
            { 
                return await _shoppingCartRepository.GetCartByUserId(userId);
            }
            else
            { 
                return GetCartFromCookies(userId);
            }
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

        private ShoppingCart GetCartFromCookies(string userGuid)
        {
            var cookieValue = Request.Cookies[CartCookieKey + "_" + userGuid];
            if (!string.IsNullOrEmpty(cookieValue))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cookieValue) ?? new ShoppingCart();
            }
            return new ShoppingCart();
        }

        private Task SaveCartToCookies(string userGuid, ShoppingCart cart)
        {
            var serializedCart = JsonSerializer.Serialize(cart);

            var cookieOptions = new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = false,
                Expires = DateTime.Now.AddDays(7)
            };

            Response.Cookies.Append(
                CartCookieKey + "_" + userGuid,
                serializedCart,
                cookieOptions
            );

            return Task.CompletedTask;
        }

    }
}