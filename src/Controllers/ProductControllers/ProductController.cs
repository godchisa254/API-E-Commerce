using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Controllers.Product
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
            return Ok(products);
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
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        // [Authorize (Roles = "Admin")] 
        public async Task<IActionResult> Post([FromForm] CreateProductRequestDto request)
        {
            Console.WriteLine($"Product Model: asd");
            if(request.Image == null || request.Image.Length == 0)
            {
                return BadRequest("Image is required");
            }

            if(request.Image.ContentType  != "image/jpg" && request.Image.ContentType != "image/png")
            {
                return BadRequest("Image must be a jpg or png");
            }

            if(request.Image.Length > 10 * 1024 * 1024)
            {
                return BadRequest("Image must be less than 10MB");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.Image.FileName, request.Image.OpenReadStream()),
                Folder = "products_image"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if(uploadResult.Error != null)
            {
                return BadRequest(uploadResult.Error.Message);
            }

            var productModel = request.ToProduct(uploadResult.SecureUrl.AbsoluteUri);

            
            await _productRepository.CreateProduct(productModel);
            return CreatedAtAction(nameof(GetById), new { id = productModel.ID }, productModel.ToGetProductDto());
        }

        
    }
}