using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public int? ProductTypeID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public decimal? Price { get; set; }        
        public int? Stock { get; set; } 
        public string? Image { get; set; } = string.Empty;
    }
}