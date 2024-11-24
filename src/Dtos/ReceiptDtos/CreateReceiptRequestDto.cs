namespace taller1.src.Dtos.ReceiptDtos
{
    /// <summary>
    /// DTO utilizado para obtener la dirección del usuario que se agregará a la boleta.
    /// </summary>
    public class CreateReceiptRequestDto
    {

        /// <summary>
        /// País asociado a la dirección de la boleta.
        /// </summary>
        public string Country { get; set; } = string.Empty; 

        /// <summary>
        /// Ciudad asociada a la dirección de la boleta.
        /// </summary>
        public string City { get; set; } = string.Empty; 

        /// <summary>
        /// Comuna asociada a la dirección de la boleta.
        /// </summary>
        public string Commune { get; set; } = string.Empty;  

        /// <summary>
        /// Calle asociada a la dirección de la boleta.
        /// </summary>
        public string Street { get; set; } = string.Empty;
        
    }
}