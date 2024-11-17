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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly Cloudinary _cloudinary;
        public ProductController(IProductRepository productRepository, Cloudinary cloudinary)
        {
            _productRepository = productRepository;
            _cloudinary = cloudinary;
        }

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

            var products = await _productRepository.GetAll(query);
            var productDtos = products.Select(x => x.ToGetProductDto()).ToList();

            return Ok(productDtos);
        }

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