using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace taller1.src.Models
{
    /// <summary>
    /// Representa un usuario en la aplicación, extendiendo las propiedades base de IdentityUser.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Rut del usuario.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio, tiene un tamaño máximo de 12 caracteres y se almacena como varchar.
        /// </remarks>
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string Rut { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y tiene un tamaño máximo de 255 caracteres.
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio.
        /// </remarks>
        [Required]
        public DateOnly Birthdate { get; set; }

        /// <summary>
        /// Género del usuario.
        /// </summary>
        /// <remarks>
        /// El valor debe estar en el rango de 0 a 3, donde 0 = hombre, 1 = mujer, 2 = otro y 3 = prefiero no decirlo.
        /// </remarks>
        [Required]
        [Range(0, 4)]
        public int Gender { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio, tiene un tamaño máximo de 20 caracteres y se almacena como varchar.
        /// </remarks>
        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Token de autenticación del usuario.
        /// </summary>
        /// <remarks>
        /// Este campo no es obligatorio.
        /// </remarks>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el usuario está habilitado para iniciar sesión en el sistema.
        /// </summary>
        /// <remarks>
        /// Valor por defecto: true.
        /// </remarks>
        public bool enabledUser { get; set; } = true;

        /// <summary>
        /// Propiedad de navegación de lista de recibos asociados al usuario.
        /// </summary>
        public List<Receipt> Receipts { get; set; } = [];

        /// <summary>
        /// Propiedad de navegación de carrito de compras asociado al usuario.
        /// </summary>
        /// <remarks>
        /// Este campo es virtual para soporte de lazy loading y puede ser nulo.
        /// </remarks>
        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}
