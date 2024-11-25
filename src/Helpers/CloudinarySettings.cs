namespace taller1.src.Helpers
{
    /// <summary>
    /// Clase que contiene la configuración necesaria para interactuar con el servicio Cloudinary.
    /// </summary>
    public class CloudinarySettings
    {
        /// <summary>
        /// El nombre de la cuenta de Cloudinary.
        /// </summary>
        public string CloudName { get; set; } = string.Empty;

        /// <summary>
        /// La clave de la API para autenticar las solicitudes hacia Cloudinary.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// El secreto de la API para autenticar las solicitudes hacia Cloudinary.
        /// </summary>
        public string ApiSecret { get; set; } = string.Empty;
    }
}
