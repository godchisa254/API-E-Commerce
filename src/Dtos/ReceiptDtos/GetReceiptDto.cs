using taller1.src.Models;

namespace taller1.src.Dtos.ReceiptDtos
{
    public class GetReceiptDto
    { 
        public int ID { get; set; } 
        public string UserID { get; set; } = string.Empty; 
        public string Country { get; set; } = string.Empty; 
        public string City { get; set; } = string.Empty; 
        public string Commune { get; set; } = string.Empty;  
        public string Street { get; set; } = string.Empty; 
        public DateOnly Date { get; set; } 
        public float Total { get; set; } 
        public required List<ReceiptItemDetail> ReceiptItemDetails { get; set; }
        
    }
}