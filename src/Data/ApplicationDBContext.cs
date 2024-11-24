using taller1.src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace taller1.src.Data
{
    /// <summary>
    /// Contexto principal de la base de datos que extiende <see cref="IdentityDbContext{AppUser}"/> 
    /// y maneja la configuración de las tablas y las relaciones entre las entidades.
    /// </summary>
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// Constructor que recibe las opciones de configuración para el contexto de la base de datos.
        /// </summary>
        /// <param name="options">Opciones de configuración para el contexto de la base de datos.</param>
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        /// <summary>
        /// Tabla de <see cref="ShoppingCart"/> que representa los carritos de compra de los usuarios.
        /// </summary>
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;

        /// <summary>
        /// Tabla de <see cref="ShoppingCartItem"/> que representa los productos dentro de un carrito de compras.
        /// </summary>
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;

        /// <summary>
        /// Tabla de <see cref="Receipt"/> que representa las boletas generadas por compras.
        /// </summary>
        public DbSet<Receipt> Receipts { get; set; } = null!;

        /// <summary>
        /// Tabla de <see cref="ReceiptItemDetail"/> que contiene los detalles de los productos comprados.
        /// </summary>
        public DbSet<ReceiptItemDetail> ReceiptItemDetails { get; set; } = null!;

        /// <summary>
        /// Tabla de <see cref="Product"/> que contiene los productos disponibles.
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Tabla de <see cref="ProductType"/> que contiene los tipos de productos disponibles.
        /// </summary>
        public DbSet<ProductType> ProductTypes { get; set; } = null!;

        /// <summary>
        /// Configuración del modelo de datos para establecer relaciones entre las entidades y establecer valores predeterminados.
        /// </summary>
        /// <param name="modelBuilder">El <see cref="ModelBuilder"/> usado para configurar las entidades y sus relaciones.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Carga datos iniciales para los roles de usuario.
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole {Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole {Id = "2", Name = "User", NormalizedName = "USER"}
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Configura la relación entre los usuarios y sus carritos de compra.
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(sc => sc.AppUser)
                .HasForeignKey<ShoppingCart>(sc => sc.UserID)
                .HasPrincipalKey<AppUser>(u => u.Id);
        }
    }
}
