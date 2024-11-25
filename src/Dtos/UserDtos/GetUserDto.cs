namespace taller1.src.Dtos.UserDtos
{
    /// <summary>
    /// DTO utilizado para obtener un usuario.
    /// </summary>
    public class GetUserDto
    { 
        /// <summary>
        /// Rut del usuario.
        /// </summary>
        public string Rut { get; set; } = string.Empty; 

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        public string Name { get; set; } = string.Empty; 

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        public DateOnly Birthdate { get; set; } 

        /// <summary>
        /// Género del usuario.
        /// </summary>
        /// <remarks>
        /// El valor debe estar en el rango de 0 a 3, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decirlo.
        /// </remarks>
        public int Gender { get; set; }  

        /// <summary>
        /// Indica si el usuario está habilitado para iniciar sesión en el sistema.
        /// </summary>
        /// <remarks>
        /// Valor por defecto: true.
        /// </remarks>
        public bool enabledUser { get; set; } = true; 

        /// <summary>
        /// Dirección de correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
    }
}