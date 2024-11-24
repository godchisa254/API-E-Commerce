using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    public static class ReceiptMapper
    {
        
        // Mapeo de receiptModel a GetReceiptDto
        public static GetReceiptDto ToGetReceiptDto(this Receipt receiptModel)
        {
            return new GetReceiptDto
            {
                ID = receiptModel.ID,
                UserID = receiptModel.UserID,
                Country = receiptModel.Country,
                City = receiptModel.City,
                Commune = receiptModel.Commune,
                Street = receiptModel.Street,
                Date = receiptModel.Date,
                Total = receiptModel.Total,
                ReceiptItemDetailsDto = receiptModel.ReceiptItemDetails.Select(item => item.ToReceiptItemDetailsDto()).ToList()
            };
        }
        // Mapeo de CreateReceiptRequestDto a receiptModel
        public static Receipt ToReceiptModel(this CreateReceiptRequestDto createReceiptDto, ShoppingCart shoppingCart)
        {
            var total = shoppingCart.ShoppingCartItems.Sum(item => item.Product.Price * item.Quantity);

            var receiptItems = shoppingCart.ShoppingCartItems
                .Select(item => new ReceiptItemDetail
                {
                    ProductID = item.ProductID,
                    Name = item.Product.Name,
                    Type = item.Product.ProductType.Type,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.Product.Price
                })
                .ToList();

            return new Receipt
            {
                UserID = shoppingCart.UserID,
                Country = createReceiptDto.Country,
                City = createReceiptDto.City,
                Commune = createReceiptDto.Commune,
                Street = createReceiptDto.Street,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Total = total
            };
        }
        // Mapeo de receiptItemDetailModel a ReceiptItemDetailDto
        public static ReceiptItemDetailDto ToReceiptItemDetailsDto(this ReceiptItemDetail receiptItemDetailsModel)
        {
            return new ReceiptItemDetailDto
            {
                ProductID = receiptItemDetailsModel.ProductID,
                Name = receiptItemDetailsModel.Name,
                Type = receiptItemDetailsModel.Type,
                UnitPrice = receiptItemDetailsModel.UnitPrice,
                Quantity = receiptItemDetailsModel.Quantity,
                TotalItemPrice = receiptItemDetailsModel.TotalPrice,
                Image = receiptItemDetailsModel.Product.Image
            };
        }
    }
}