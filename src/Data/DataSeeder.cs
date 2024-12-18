using taller1.src.Dtos.ProductDtos;
using taller1.src.Interface;
using taller1.src.Mappers;
using taller1.src.Models;

namespace taller1.src.Data
{
    /// <summary>
    /// Clase encargada de la inicialización y si es necesario, la creación de datos predeterminados en la base de datos, 
    /// como un administrador y tipos y productos de ejemplo.
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Repositorio que maneja las operaciones relacionadas con el usuario administrador y los roles.
        /// </summary>
        private readonly ISeederRepository _seederRepository;

        /// <summary>
        /// Constructor de la clase DataSeeder.
        /// </summary>
        /// <param name="seederRepository">Repositorio del Seeder para interactuar con la base de datos de usuarios y roles.</param>
        public DataSeeder(ISeederRepository seederRepository)
        {
            _seederRepository = seederRepository;
        }

        /// <summary>
        /// Método encargado de crear un usuario administrador si no existe uno en el sistema.
        /// </summary>
        /// <returns>Tarea asincrónica.</returns>
        public async Task createAdmin()
        {
            // Verifica si ya existe un administrador con el rol asignado.
            if (await _seederRepository.GetAdminByRol() != null)
            {
                return; // Si ya existe, no hace nada.
            }

            // Obtiene la información del administrador desde las variables de entorno.
            string adminName = Environment.GetEnvironmentVariable("ADMIN_NAME")!;
            string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL")!;
            string adminRut = Environment.GetEnvironmentVariable("ADMIN_RUT")!;
            string adminBirthdate = Environment.GetEnvironmentVariable("ADMIN_BIRTHDATE")!;
            string adminGender = Environment.GetEnvironmentVariable("ADMIN_GENDER")!;

            // Crea una nueva instancia de AppUser con los datos proporcionados.
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Rut = adminRut,
                Name = adminName,
                Birthdate = DateOnly.Parse(adminBirthdate),
                Gender = int.Parse(adminGender)
            };

            // Obtiene la ministrador desde las variables de entorno.
            string adminpass = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!;

            // Crea el administrador en el sistema.
            var createUser = await _seederRepository.CreateAdminAsync(admin, adminpass);

            // Si la creación fue exitosa, se le asigna el rol de administrador.
            if (createUser.Succeeded)
            {
                await _seederRepository.AddRole(admin, "Admin");
            }
        }

        /// <summary>
        /// Método estático que inicializa los datos de la base de datos llamando a la creación de productos y tipos de productos.
        /// </summary>
        /// <param name="serviceProvider">Proveedor de servicios para acceder a los servicios registrados en el contenedor de dependencias.</param>
        /// <returns>Tarea asincrónica.</returns>
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();

                // Llama a los métodos que agregan datos iniciales.
                await SeedProductTypes(context);
                await SeedProducts(context);

                // Realiza cambios en la base de datos (aunque no se agrega nada adicional en este punto).
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Método que agrega tipos de productos a la base de datos si no existen previamente.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <returns>Tarea asincrónica.</returns>
        public static async Task SeedProductTypes(ApplicationDBContext context)
        {
            if (!context.ProductTypes.Any())
            {
                var types = new List<ProductType>
                {
                    new ProductType { Type = "Poleras" },
                    new ProductType { Type = "Gorros" },
                    new ProductType { Type = "Juguetería" },
                    new ProductType { Type = "Alimentación" },
                    new ProductType { Type = "Libros" }
                };

                // Verifica que no se agreguen duplicados antes de insertar.
                foreach (var type in types)
                {
                    if (!context.ProductTypes.Any(c => c.Type == type.Type))
                    {
                        context.ProductTypes.Add(type);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Método que agrega productos a la base de datos si no existen previamente.
        /// </summary>
        /// <param name="context">El contexto de la base de datos.</param>
        /// <returns>Tarea asincrónica.</returns>
        public static async Task SeedProducts(ApplicationDBContext context)
        {
            if (!context.Products.Any())
            {
                var productsDtos = new List<GetProductDto>
                {
                    new GetProductDto { ProductTypeID = 1, Name = "Polera de algodón", Price = 10000, Stock = 10, Image = "https://fakelink.photos/200" },
                    new GetProductDto { ProductTypeID = 2, Name = "Gorro de lana", Price = 5000, Stock = 5, Image = "https://fakelink.photos/201" },
                    new GetProductDto { ProductTypeID = 3, Name = "Pelota de fútbol", Price = 15000, Stock = 20, Image = "https://fakelink.photos/202" },
                    new GetProductDto { ProductTypeID = 4, Name = "Leche", Price = 2000, Stock = 30, Image = "https://fakelink.photos/203" },
                    new GetProductDto { ProductTypeID = 5, Name = "Cien años de soledad", Price = 25000, Stock = 10, Image = "https://fakelink.photos/204" }
                };

                // Convierte los DTOs a modelos de producto y los agrega a la base de datos.
                var productModels = productsDtos.Select(p => p.ToProductFromGetDto()).ToList();
                foreach (var product in productModels)
                {
                    await context.Products.AddAsync(product);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
