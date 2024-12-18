using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers; 

namespace taller1.src.Controllers
{
    /// <summary>
    /// Controlador para la gestión de productos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de productos.
        /// </summary>
        /// <param name="productRepository">Repositorio de productos.</param>
        /// <param name="cloudinary">Instancia de Cloudinary para la carga de imágenes.</param>
        public ProductController(IProductRepository productRepository, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Obtiene todos los productos con soporte para paginación, ordenamiento y filtrado.
        /// </summary>
        /// <param name="query">Objeto de consulta para ordenar, filtrar y paginar los resultados.</param>
        /// <returns>Lista de productos en formato DTO.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                var validSortProperties = new[] { "ProductTypeID", "Name", "Price", "Stock" };
                if (!validSortProperties.Contains(query.SortBy))
                {
                    return BadRequest($"Invalid SortBy property: {query.SortBy}. Use one of the following: {string.Join(", ", validSortProperties)}");
                }
            }

            var (products, totalCount) = await _productRepository.GetAll(query);
            var productDtos = products.Select(x => x.ToGetProductDto()).ToList();
            var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

            var response = new
            {
                Items = productDtos,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        /// <summary>
        /// Obtiene un producto específico por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>Producto en formato DTO si se encuentra, o un error 404 si no se encuentra.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = product.ToGetProductDto();
            return Ok(productDto);
        }

        /// <summary>
        /// Crea un nuevo producto con la información proporcionada.
        /// </summary>
        /// <param name="request">DTO que contiene los datos del producto a crear.</param>
        /// <returns>Producto creado con el código de estado 201.</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize (Roles = "Admin")] 
        public async Task<IActionResult> Post([FromForm] CreateProductRequestDto request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            string? imageUrl = await UploadImage(request.Image);
            if (imageUrl == null)
            {
                return BadRequest("Image must be a jpg or png and less than 10MB");
            }
            var productModel = request.ToProduct(imageUrl);

            bool exists = await _productRepository.ProductExists(productModel.Name, productModel.ProductTypeID);
            if (exists)
            {
                return BadRequest("A product with the same name and type already exists.");
            }

            await _productRepository.CreateProduct(productModel);
            return CreatedAtAction(nameof(GetById), new { id = productModel.ID }, productModel.ToGetProductDto());
        }

        /// <summary>
        /// Actualiza un producto existente con los nuevos datos.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="request">DTO que contiene los datos actualizados del producto.</param>
        /// <returns>Producto actualizado en formato DTO, o error 404 si no se encuentra.</returns>
        [HttpPut]
        [Route("{id:int}")]
        [Consumes("multipart/form-data")]
        [Authorize (Roles = "Admin")] 
        public async Task<IActionResult> Put([FromRoute] int id, [FromForm] UpdateProductRequestDto request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool exists = await _productRepository.ProductExists(request.Name, request.ProductTypeID);
            if (exists)
            {
                return BadRequest("A product with the same name and type already exists.");
            }
            string? imageUrl = null;
            if (request.Image != null)
            {
                imageUrl = await UploadImage(request.Image);
                
                if (imageUrl == null)
                {
                    return BadRequest("Image must be a jpg or png and less than 10MB");
                }
            }

            var productModel = await _productRepository.UpdateProduct(id, request, imageUrl);

            if (productModel == null)
            {
                return NotFound();
            }
            return Ok(productModel.ToGetProductDto());
        }

        /// <summary>
        /// Elimina un producto específico por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>Producto eliminado en formato DTO, o error 404 si no se encuentra.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productRepository.DeleteProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product.ToGetProductDto());
        }

        /// <summary>
        /// Carga una imagen para un producto en Cloudinary.
        /// </summary>
        /// <param name="image">Imagen que se va a cargar.</param>
        /// <returns>URL de la imagen cargada o null si hay un error.</returns>
        private async Task<string?> UploadImage(IFormFile image)
        {
            if(image == null || image.Length == 0)
            {
                return null;
            }

            if(image.ContentType  != "image/jpg" && image.ContentType != "image/png")
            {
                return null;
            }

            if(image.Length > 10 * 1024 * 1024)
            {
                return null;
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                Folder = "products_image"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if(uploadResult.Error != null)
            {
                return null;
            } 

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
