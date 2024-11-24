using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using taller1.src.Dtos;
using taller1.src.Dtos.AuthDtos;
using taller1.src.Interface;
using taller1.src.Dtos.ProductDtos;
using taller1.src.Mappers;


namespace taller1.src.Data
{
    
    public class DataSeeder
    {

        private readonly ISeederRepository _seederRepository;

        private readonly ITokenService _tokenService;

        public DataSeeder(ISeederRepository seederRepository, ITokenService tokenService)
        {
            _seederRepository = seederRepository;
            _tokenService = tokenService;
        }


        public async Task createAdmin()
        {

            if(await _seederRepository.GetAdminByRol() != null)
            {
                return;
            }

            string adminName = Environment.GetEnvironmentVariable("ADMIN_NAME")!;
            string adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL")!;
            string adminRut = Environment.GetEnvironmentVariable("ADMIN_RUT")!;
            string adminBirthdate = Environment.GetEnvironmentVariable("ADMIN_BIRTHDATE")!;
            string adminGender = Environment.GetEnvironmentVariable("ADMIN_GENDER")!;

            //agregar admin
            var admin = new AppUser
            {
                UserName = adminEmail,  
                Email = adminEmail,
                Rut = adminRut,
                Name = adminName,  
                Birthdate =  DateOnly.Parse(adminBirthdate),
                Gender = int.Parse(adminGender)
            };

            string adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")!;

            //Crear Admin
            var createUser = await _seederRepository.CreateAdminAsync(admin, adminPassword);

            //Asignarle rol admin

            if(createUser.Succeeded)
            {
                var role = await _seederRepository.AddRole(admin, "Admin");

            }

        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)

        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();

                await SeedProductTypes(context);
                await SeedProducts(context);

                var existingRuts = new HashSet<string>(); 

                context.SaveChanges();

            }
        } 
        
        private static string GenerateUniqueRandomRut(HashSet<string> existingRuts)
        {
            string rut;
            do
            {
                rut = GenerateRandomRut();
            } while (existingRuts.Contains(rut));
            existingRuts.Add(rut);
            return rut;
        }

        private static string GenerateRandomRut()
        {
            Random random = new();
            int rutNumber = random.Next(1, 99999999);
            int verificator = CalculateRutVerification(rutNumber);
            string verificatorStr = verificator.ToString();
            if(verificator == 10)
            {
                verificatorStr = "k";
            }
            return $"{rutNumber}-{verificatorStr}";
        }

        private static int CalculateRutVerification(int rutNumber)
        {
            int[] coefficients = {2, 3, 4, 5, 6, 7};
            int sum = 0;
            int index = 0;

            while(rutNumber != 0)
            {
                sum += rutNumber % 10 * coefficients[index];
                rutNumber /= 10;
                index = (index + 1) % 6;
            }
            int verification = 11 - (sum % 11);
            return verification == 11 ? 0 : verification;
        }

        public static async Task SeedProductTypes(ApplicationDBContext context)
        {
        
            if(!context.ProductTypes.Any())
            {
                var types = new List<ProductType>
                {
                    new ProductType { Type = "Poleras" },
                    new ProductType { Type = "Gorros" },
                    new ProductType { Type = "Juguetería" },
                    new ProductType { Type = "Alimentación" },
                    new ProductType { Type = "Libros" }
                };

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

        public static async Task SeedProducts(ApplicationDBContext context)
        {
            if(!context.Products.Any())
            {
                var productsDtos = new List<GetProductDto>
                {
                    new GetProductDto { ProductTypeID = 1, Name = "Polera de algodón", Price = 10000, Stock = 10, Image = "https://fakelink.photos/200" },
                    new GetProductDto { ProductTypeID = 2, Name = "Gorro de lana", Price = 5000, Stock = 5, Image = "https://fakelink.photos/201"  },
                    new GetProductDto { ProductTypeID = 3, Name = "Pelota de fútbol", Price = 15000, Stock = 20, Image = "https://fakelink.photos/202"  },
                    new GetProductDto { ProductTypeID = 4, Name = "Leche", Price = 2000, Stock = 30, Image = "https://fakelink.photos/203"  },
                    new GetProductDto { ProductTypeID = 5, Name = "Cien años de soledad", Price = 25000, Stock = 10, Image = "https://fakelink.photos/204"  }
                };

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