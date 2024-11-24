namespace taller1.src.Helpers
{
    public class QueryReceipt : QueryObject
    {  
        public string? UserID { get; set; } = string.Empty; 
        // public string? Country { get; set; } = string.Empty; 
        // public string? City { get; set; } = string.Empty; 
        // public string? Commune { get; set; } = string.Empty; 
        // public string? Street { get; set; } = string.Empty; 
        public DateOnly? Date { get; set; } 
        // public float? Total { get; set; }  

    }
}