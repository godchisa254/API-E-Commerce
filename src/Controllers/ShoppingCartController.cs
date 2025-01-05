using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos.shoppingCartDto;
using taller1.src.Interface;
using taller1.src.Models;

namespace taller1.src.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar el carrito de compras del usuario.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    { 
        private const string CartCookieKey = "TicketsList"; 
        private const string UserCookieKey = "UserGUID";
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor del controlador. 
        /// </summary>
        /// <param name="shoppingCartRepository">Repositorio de carrito de compras.</param>
        /// <param name="productRepository">Repositorio de productos.</param>
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Obtiene el carrito de compras del usuario actual.
        /// </summary>
        /// <returns>Un resultado con los productos del carrito de compras.</returns>
        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetCart()
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new {message="User is not authenticated."});
            }

            try
            { 
                var cart = await GetCart(userId);

                if (cart == null)
                {
                    return NotFound(new {message ="Shopping cart not found or is empty."});
                }

                var cartItems = new List<object>();

                foreach (var item in cart.ShoppingCartItems)
                {
                    var product = await _productRepository.GetById(item.ProductID);

                    if (product == null)
                    {
                        return BadRequest(new {message =$"Product with ID {item.ProductID} does not exist."});
                    }

                    cartItems.Add(new
                    {
                        product.ID,
                        product.Name,
                        product.Price,
                        product.Image,
                        item.Quantity,
                        TotalPrice = product.Price * item.Quantity
                    });
                }

                if (cartItems.Count == 0)
                {
                    return BadRequest( new {message ="No valid products in the shopping cart."});
                }

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message =$"An error occurred: {ex.Message}"});
            }
        }


        /**
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

                if (cart == null)
                {
                    return NotFound("Shopping cart not found or is empty.");
                }
                var productId = cart.ShoppingCartItems.FirstOrDefault()?.ProductID;
                if (productId == null)
                {
                    return BadRequest("Product ID not found in the shopping cart items.");
                }

                var product = await _productRepository.GetById(productId.Value);

                if (product == null)
                {
                    return BadRequest("Product does not exist.");
                }
                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    product.ID,
                    product.Name,
                    product.Price,
                    product.Image,
                    item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        **/
        
        /// <summary>
        /// Añade un producto al carrito de compras del usuario.
        /// </summary>
        /// <param name="productId">ID del producto a agregar.</param>
        /// <param name="quantity">Cantidad del producto a agregar.</param>
        /// <returns>Un resultado con los productos actualizados del carrito.</returns>
        [HttpPost("add_product")]
        [AllowAnonymous]
        public async Task<IActionResult> AddToCart([FromBody]ShoppingCartDto cartDto)
        {
            string? userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized(new {message ="User is not authenticated."});
            }

            try
            { 
                
                var cart = await GetCart(userId);

                if (cart == null)
                {
                    return NotFound(new {message="Shopping cart not found or is empty."});
                }

                var existingItem = cart.ShoppingCartItems
                    .FirstOrDefault(item => item.ProductID == cartDto.ProductId);
                var product = await _productRepository.GetById(cartDto.ProductId); 
                if (product == null)
                {
                    return BadRequest(new {message="Product does not exist."});
                }

                if (existingItem != null)
                {
                    if (product.Stock < existingItem.Quantity + cartDto.Quantity)
                    {
                        return BadRequest(new {message="Not enough stock available."});
                    }
                    existingItem.Quantity += cartDto.Quantity;
                }
                else
                { 
                    if (product == null)
                    {
                        return NotFound(new {message = "Product not found."});
                    }
                    if (product.Stock < cartDto.Quantity)
                    {
                        return BadRequest(new {message = $"Not enough stock available for the product {product.Name}."});
                    }
                    //TODO: use mapper
                    cart.ShoppingCartItems.Add(new ShoppingCartItem
                    {
                        ShoppingCartID = cart.ID,
                        ProductID = product.ID,
                        Quantity = cartDto.Quantity,
                    });
                    Console.WriteLine("Product added to cart");
                } 

                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    product.Name,
                    product.Price,
                    item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message =$"An error occurred: {ex.Message}"});
            }
        }
        
        /// <summary>
        /// Elimina una cantidad de un producto del carrito de compras del usuario.
        /// </summary>
        /// <param name="productId">ID del producto a eliminar.</param>
        /// <param name="quantity">Cantidad del producto a eliminar.</param>
        /// <returns>Un resultado con los productos actualizados del carrito.</returns>
        [HttpPost("deduct_product")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveFromCart([FromBody]ShoppingCartDto cartDto)
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
                    return NotFound(new  { Message ="Shopping cart is empty."});
                }

                var existingItem = cart.ShoppingCartItems
                    .FirstOrDefault(item => item.ProductID == cartDto.ProductId);
                var product = await _productRepository.GetById(cartDto.ProductId); 
                if (product == null)
                {
                    return BadRequest(new { message ="Product does not exist."});
                }

                if (existingItem == null)
                {
                    return NotFound(new { message ="Product not found in the cart."});
                }
 
                existingItem.Quantity -= cartDto.Quantity;
                if (existingItem.Quantity <= 0)
                {
                    cart.ShoppingCartItems.Remove(existingItem);
                } 

                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    product.Name,
                    product.Price,
                    item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}"});
            }
        } 

        /// <summary>
        /// Elimina un producto del carrito de compras del usuario.
        /// </summary>
        /// <param name="productId">ID del producto a eliminar.</param>
        /// <returns>Un resultado con los productos actualizados del carrito.</returns>
        [HttpDelete("remove_product")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveProduct([FromBody]int productId)
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
                var product = await _productRepository.GetById(productId); 
                if (product == null)
                {
                    return BadRequest("Product does not exist.");
                }

                if (existingItem == null)
                {
                    return NotFound("Product not found in the cart.");
                } 
                cart.ShoppingCartItems.Remove(existingItem);
 
                SaveCartAnyUser(cart, userId);

                var cartItems = cart.ShoppingCartItems.Select(item => new
                {
                    product.Name,
                    product.Price,
                    item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                });

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
 
        /// <summary>
        /// Guarda el carrito de compras de un usuario, ya sea autenticado o no.
        /// </summary>
        /// <param name="cart">El carrito de compras a guardar.</param>
        /// <param name="userId">El ID del usuario.</param>
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
        
        /// <summary>
        /// Obtiene el ID del usuario autenticado o crea uno nuevo para un usuario no autenticado.
        /// </summary>
        /// <returns>El ID del usuario.</returns>
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

        /// <summary>
        /// Obtiene el carrito de compras asociado a un usuario específico.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>El carrito de compras.</returns>
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

        /// <summary>
        /// Crea o recupera un GUID único para el usuario no autenticado.
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

        /// <summary>
        /// Obtiene el carrito de compras de un usuario desde las cookies si el usuario no está autenticado.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>El carrito de compras.</returns>
        private ShoppingCart GetCartFromCookies(string userGuid)
        {
            var cookieValue = Request.Cookies[CartCookieKey + "_" + userGuid];
            if (!string.IsNullOrEmpty(cookieValue))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cookieValue) ?? new ShoppingCart();
            }
            return new ShoppingCart();
        }

        /// <summary>
        /// Guarda el carrito de compras de un usuario en las cookies.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <param name="cart">El carrito de compras.</param>
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