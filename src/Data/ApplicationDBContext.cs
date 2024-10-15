using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller1.src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace taller1.src.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }


        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptItemDetail> ReceiptItemDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }

        
          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Seed roles in the database
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole {Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole {Id = "2", Name = "User", NormalizedName = "USER"}
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            
             modelBuilder.Entity<AppUser>()
            .HasOne(u => u.ShoppingCart)
            .WithOne(sc => sc.AppUser)
            .HasForeignKey<ShoppingCart>(sc => sc.UserID)
            .HasPrincipalKey<AppUser>(u => u.Id); 



        }

        
    }
}