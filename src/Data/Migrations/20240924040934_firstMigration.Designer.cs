﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using taller1.src.Data;

#nullable disable

namespace taller1.src.Data.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20240924040934_firstMigration")]
    partial class firstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("taller1.src.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(65)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<int>("ProductTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Stock")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ProductTypeID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("taller1.src.Models.ProductType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("taller1.src.Models.Receipt", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Commune")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("taller1.src.Models.ReceiptItemDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(65)
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReceiptId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ReceiptId");

                    b.ToTable("ReceiptItemDetails");
                });

            modelBuilder.Entity("taller1.src.Models.ShoppingCart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("taller1.src.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShoppingCartID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ShoppingCartID");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("taller1.src.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("Rut")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar");

                    b.Property<int>("UserRoleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("UserRoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("taller1.src.Models.UserRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("taller1.src.Models.Product", b =>
                {
                    b.HasOne("taller1.src.Models.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("taller1.src.Models.Receipt", b =>
                {
                    b.HasOne("taller1.src.Models.User", "User")
                        .WithMany("Receipts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("taller1.src.Models.ReceiptItemDetail", b =>
                {
                    b.HasOne("taller1.src.Models.Product", "Product")
                        .WithMany("ReceiptItemDetails")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taller1.src.Models.Receipt", "Receipt")
                        .WithMany("ReceiptItemDetails")
                        .HasForeignKey("ReceiptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Receipt");
                });

            modelBuilder.Entity("taller1.src.Models.ShoppingCart", b =>
                {
                    b.HasOne("taller1.src.Models.User", "User")
                        .WithOne("ShoppingCart")
                        .HasForeignKey("taller1.src.Models.ShoppingCart", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("taller1.src.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("taller1.src.Models.Product", "Product")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("taller1.src.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ShoppingCartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("taller1.src.Models.User", b =>
                {
                    b.HasOne("taller1.src.Models.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("taller1.src.Models.Product", b =>
                {
                    b.Navigation("ReceiptItemDetails");

                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("taller1.src.Models.ProductType", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("taller1.src.Models.Receipt", b =>
                {
                    b.Navigation("ReceiptItemDetails");
                });

            modelBuilder.Entity("taller1.src.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("taller1.src.Models.User", b =>
                {
                    b.Navigation("Receipts");

                    b.Navigation("ShoppingCart")
                        .IsRequired();
                });

            modelBuilder.Entity("taller1.src.Models.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
