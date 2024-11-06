using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Helpers
{
   
    public class QueryObject
    {
        public string? Name { get; set; } = string.Empty;
        public string? Price { get; set; } = string.Empty;
        public string? Stock { get; set; } = string.Empty;
        public string? SortBy { get; set; } = string.Empty;
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
    }
}