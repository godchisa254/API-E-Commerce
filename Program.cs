using System.Text;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using taller1.src.Data;
using taller1.src.Helpers;
using taller1.src.Interface;
using taller1.src.Models;
using taller1.src.Repository;
using taller1.src.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
    var cloudinaryAccount = new Account(
        cloudinarySettings!.CloudName,
        cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret
    );
    var cloudinary = new Cloudinary(cloudinaryAccount);
    builder.Services.AddSingleton(cloudinary);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt =>
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 8;
    }
).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication( 
    opt =>
    {
        opt.DefaultAuthenticateScheme = 
        opt.DefaultChallengeScheme = 
        opt.DefaultForbidScheme = 
        opt.DefaultScheme = 
        opt.DefaultSignInScheme = 
        opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(
    opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidateAudience = true,
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentNullException("JWT: SigningKey"))),
        };
    }
);

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source = app.db";
builder.Services.AddDbContext<ApplicationDBContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISeederRepository, SeederRepository>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

using(var scope = app.Services.CreateScope())
{
    
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();

    await context.Database.MigrateAsync();
    
    //Seeder
    var dataSeeder = services.GetRequiredService<DataSeeder>();
    await dataSeeder.createAdmin();

    await context.Database.MigrateAsync();   
    await DataSeeder.InitializeAsync(services);

}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
