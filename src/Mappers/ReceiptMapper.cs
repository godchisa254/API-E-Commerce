using taller1.src.Dtos.ReceiptDtos;
using taller1.src.Models;

namespace taller1.src.Mappers
{
    /// <summary>
    /// Proporciona métodos para mapear entre la entidad <see cref="Receipt"/> y sus DTOs.
    /// </summary>
    public static class ReceiptMapper
    {
        /// <summary>
        /// Convierte un modelo de boleta en un DTO para la respuesta.
        /// </summary>
        /// <param name="receiptModel">El modelo de boleta a convertir.</param>
        /// <returns>Un objeto <see cref="GetReceiptDto"/> que representa la boleta.</returns>
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

        /// <summary>
        /// Convierte un DTO de creación de boleta en un modelo de boleta.
        /// </summary>
        /// <param name="createReceiptDto">El DTO con los datos de la boleta.</param>
        /// <param name="shoppingCart">El carrito de compras asociado a la boleta.</param>
        /// <returns>Un objeto <see cref="Receipt"/> inicializado con los datos proporcionados.</returns>
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

        /// <summary>
        /// Convierte un modelo de detalles de artículo de recibo en un DTO.
        /// </summary>
        /// <param name="receiptItemDetailsModel">El modelo de detalles del artículo del recibo.</param>
        /// <returns>Un objeto <see cref="ReceiptItemDetailDto"/> que representa el artículo del recibo.</returns>
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
