namespace taller1.src.Helpers
{
    /// <summary>
    /// Clase que extiende <see cref="BaseQuery"/> e incluye un campo adicional para filtrar por tipo de producto.
    /// </summary>
    public class QueryObject : BaseQuery
    {
        /// <summary>
        /// El ID del tipo de producto para filtrar los resultados.
        /// </summary>
        public int? ProductTypeID { get; set; }
    }
}
