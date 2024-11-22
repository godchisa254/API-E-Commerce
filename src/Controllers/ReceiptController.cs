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
        private readonly IReceiptRepository _receiptRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAuthRepository _authRepository;
        public ReceiptController(IReceiptRepository receiptRepository, IShoppingCartRepository shoppingCartRepository, IAuthRepository authRepository)
        {
            _receiptRepository = receiptRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _authRepository = authRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryReceipt query)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var receipts = await _receiptRepository.GetAll(query);
            return Ok(receipts); //TODO: retornar DTO, no el modelo
        } 
        


    }
}