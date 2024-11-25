namespace taller1.src.Helpers
{
    /// <summary>
    /// Clase que extiende <see cref="QueryObject"/> e incluye propiedades adicionales para filtrar recibos por usuario y fecha.
    /// </summary>
    public class QueryReceipt : QueryObject
    {  
        /// <summary>
        /// El ID del usuario que realizó el recibo. Permite filtrar los recibos por usuario.
        /// </summary>
        public string? UserID { get; set; } = string.Empty;

        /// <summary>
        /// La fecha de los recibos. Permite filtrar los recibos por una fecha específica.
        /// </summary>
        public DateOnly? Date { get; set; }
    }
}
