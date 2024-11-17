
namespace taller1.src.Dtos.ProductDtos
{
    public class GetProductDto
    {
        public int ProductTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}