namespace taller1.src.Dtos.ReceiptDtos
{
    /// <summary>
    /// DTO utilizado para obtener una boleta y sus productos.
    /// </summary>
    public class GetReceiptDto
    { 
        /// <summary>
        /// Identificador único de la boleta.
        /// </summary>
        public int ID { get; set; } 

        /// <summary>
        /// Identificador único del usuario asociado a la boleta.
        /// </summary>
        /// <remarks>
        /// Es una clave foránea que referencia a la entidad <see cref="AppUser"/>.
        /// </remarks>
        public string UserID { get; set; } = string.Empty; 

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

        /// <summary>
        /// Fecha en que se generó la boleta.
        /// </summary>
        public DateOnly Date { get; set; } 

        /// <summary>
        /// Monto total de la compra.
        /// </summary>
        public float Total { get; set; } 

        /// <summary>
        /// Detalles de los productos asociados a la boleta.
        /// </summary>
        public required List<ReceiptItemDetailDto> ReceiptItemDetailsDto { get; set; }
        
    }
}