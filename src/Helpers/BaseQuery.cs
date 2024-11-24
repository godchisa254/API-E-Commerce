namespace taller1.src.Helpers
{
    /// <summary>
    /// Clase base que contiene propiedades comunes para consultas que soportan paginación y ordenación.
    /// </summary>
    public class BaseQuery
    {
        /// <summary>
        /// Nombre o término de búsqueda para filtrar los resultados.
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// El campo por el cual se ordenarán los resultados.
        /// </summary>
        public string? SortBy { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el ordenamiento es descendente. Si es falso, el ordenamiento es ascendente.
        /// </summary>
        public bool IsDescending { get; set; } = false;

        /// <summary>
        /// El número de página para la paginación. El valor predeterminado es 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// El tamaño de la página para la paginación. El valor predeterminado es 10.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
