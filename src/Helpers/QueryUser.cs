namespace taller1.src.Helpers
{
    /// <summary>
    /// Clase que extiende <see cref="BaseQuery"/> e incluye un campo adicional para filtrar usuarios por su estado de habilitación.
    /// </summary>
    public class QueryUser : BaseQuery
    {
        /// <summary>
        /// Indica si el usuario está habilitado o no. Permite filtrar usuarios por su estado de habilitación.
        /// </summary>
        public bool? enabledUser { get; set; }
    }
}
