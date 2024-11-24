namespace taller1.src.Dtos.ReceiptDtos
{
    public class CreateReceiptRequestDto
    {
        public string Country { get; set; } = string.Empty; 
        public string City { get; set; } = string.Empty; 
        public string Commune { get; set; } = string.Empty;  
        public string Street { get; set; } = string.Empty;
        
    }
}