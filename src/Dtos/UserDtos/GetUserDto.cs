namespace taller1.src.Dtos.UserDtos
{
    public class GetUserDto
    { 
        public string Rut { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
        public DateOnly Birthdate { get; set; } 
        public int Gender { get; set; }  
        public bool enabledUser { get; set; } = true; 
        public string Email { get; set; } = string.Empty;
        
    }
}