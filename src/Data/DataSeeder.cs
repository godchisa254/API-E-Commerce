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

                if(role.Succeeded)
                {
                    
                    new NewUserDto
                    {
                        Rut = admin.Rut,
                        Name = admin.Name,
                        Email = admin.Email,
                    };
                    
                }
            }

        }
        


       
        public static async Task InitializeAsync(IServiceProvider serviceProvider)

        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();

                await SeedProductTypes(context);

                // if(!context.UserRoles.Any())
                // {
                //     context.UserRoles.AddRange(
                //         new UserRole {Role = "Administrador"},
                //         new UserRole {Role = "Usuario"}
                //     );

                //     context.SaveChanges();
                // }

                var existingRuts = new HashSet<string>();

                // if(!context.Users.Any())
                // {
                //     var userFaker = new Faker<User>()
                //         .RuleFor(u => u.Rut , f => GenerateUniqueRandomRut(existingRuts))
                //         .RuleFor(u => u.Name , f => f.Person.FullName)
                //         .RuleFor(u => u.Birthdate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                //         .RuleFor(u => u.Email , f => f.Person.Email)
                //         .RuleFor(u => u.Gender, f => f.PickRandom(new[] { 0, 1, 2, 3 }))
                //         .RuleFor(u => u.Password, f => f.Internet.Password())
                //         .RuleFor(u => u.UserRoleID , f => f.Random.Number(1,2));
                    
                //     var users = userFaker.Generate(10);
                //     context.Users.AddRange(users);
                //     context.SaveChanges();
                // }

                // if(!context.Products.Any())
                // {
                //     var productFaker = new Faker<Product>()
                //         .RuleFor(p => p.Name , f => f.Commerce.ProductName())
                //         .RuleFor(p => p.ProductTypeID, f => f.PickRandom(new[] { 0, 1, 2, 3, 4 }))
                //         .RuleFor(p => p.Price , f => f.Random.Number(1000, 1000000))
                //         .RuleFor(p => p.Stock, f => f.Random.Number(1, 100))
                //         .RuleFor(p => p.Image, f => f.Image.PicsumUrl());
                    
                //     var products = productFaker.Generate(10);
                //     context.Products.AddRange(products);
                //     context.SaveChanges();
                // }

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


    }
}