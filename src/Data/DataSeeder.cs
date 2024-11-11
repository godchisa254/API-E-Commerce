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


namespace taller1.src.Data.Migrations
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

            //agregar admin
            var admin = new AppUser
            {
                UserName = "admin@idwm.cl",  
                Email = "admin@idwm.cl",
                Rut = "20416699-4",
                Name = "Ignacio Mancilla",  
                Birthdate =  new DateOnly(2000, 10, 25),
                Gender = 1
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
                        Token = _tokenService.CreateToken(admin)
                    };
                    
                }
            }

        }
        


        /**
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();

                if(!context.UserRoles.Any())
                {
                    context.UserRoles.AddRange(
                        new UserRole {Role = "Administrador"},
                        new UserRole {Role = "Usuario"}
                    );

                    context.SaveChanges();
                }

                var existingRuts = new HashSet<string>();

                if(!context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Rut , f => GenerateUniqueRandomRut(existingRuts))
                        .RuleFor(u => u.Name , f => f.Person.FullName)
                        .RuleFor(u => u.Birthdate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                        .RuleFor(u => u.Email , f => f.Person.Email)
                        .RuleFor(u => u.Gender, f => f.PickRandom(new[] { 0, 1, 2, 3 }))
                        .RuleFor(u => u.Password, f => f.Internet.Password())
                        .RuleFor(u => u.UserRoleID , f => f.Random.Number(1,2));
                    
                    var users = userFaker.Generate(10);
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                if(!context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Name , f => f.Commerce.ProductName())
                        .RuleFor(p => p.ProductTypeID, f => f.PickRandom(new[] { 0, 1, 2, 3, 4 }))
                        .RuleFor(p => p.Price , f => f.Random.Number(1000, 1000000))
                        .RuleFor(p => p.Stock, f => f.Random.Number(1, 100))
                        .RuleFor(p => p.Image, f => f.Image.PicsumUrl());
                    
                    var products = productFaker.Generate(10);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }

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
        **/
    }
}