namespace taller1.src.Dtos.ReceiptDtos
{
    public class ReceiptItemDetailDto
    { 
        public int ProductID { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty; 
        public float UnitPrice { get; set; } 
        public int Quantity { get; set; } 
        public float TotalItemPrice { get; set; } 
    }
}