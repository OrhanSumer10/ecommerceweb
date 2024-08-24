using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Concrete.EntityFramework.Contexts
{
    public class MySQLContext : DbContext
    {



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=ECommorceWeb;User=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 21)));
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>()
        .HasKey(c => c.CartItemId);
            // OrderDetail için bileşik anahtar tanımlaması
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId, od.ProductOptionId });

            // Order ve diğer ilişkiler
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User) //Her sipariş (Order), bir kullanıcıya (User) bağlıdır.
                .WithMany(u => u.Orders) //Bir kullanıcı birden fazla sipariş verebilir (WithMany).
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order) // Her sipariş detayı (OrderDetail), bir siparişe (Order) bağlıdır.
                .WithMany(o => o.OrderDetails) //  Bir sipariş birçok sipariş detayı içerebilir (WithMany).
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product) // Her sipariş detayı (OrderDetail), bir ürüne (Product) bağlıdır.
                .WithMany(p => p.OrderDetails) // Bir ürün birden fazla sipariş detayında bulunabilir (WithMany).
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.ProductOption)//Her sipariş detayı (OrderDetail), bir ürün seçeneğine (ProductOption) bağlıdır. 
                .WithMany(po => po.OrderDetails)// Bir ürün seçeneği birden fazla sipariş detayında bulunabilir (WithMany).
                .HasForeignKey(od => od.ProductOptionId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.ProductReviews)
                .HasForeignKey(pr => pr.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.AppliedCoupons)
                .WithMany(c => c.Orders)
                .UsingEntity(j => j.ToTable("OrderCoupons"));

            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)//Her kategori (Category), birçok alt kategoriye (SubCategories) sahip olabilir. 
                .WithOne(c => c.ParentCategory)//Alt kategoriler, bir üst kategoriye (ParentCategory) bağlıdır.
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            // Enum tanımlaması
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<int>(); // Enum'u int olarak sakla

            // WishlistItem ile ApplicationUser ilişkisini yapılandırır
            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.ApplicationUser)  // Her WishlistItem bir ApplicationUser'a sahiptir
                .WithMany(u => u.WishlistItems)  // Her ApplicationUser'un birçok WishlistItem'ı olabilir
                .HasForeignKey(w => w.ApplicationUserId)  // WishlistItem'daki ApplicationUserId, ApplicationUser tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // ApplicationUser silindiğinde ilişkili WishlistItem'lar da silinir

            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.Product)  // Her WishlistItem bir Product'a sahiptir
                .WithMany()  // Product modelinde WishlistItems koleksiyonu olmayabilir
                .HasForeignKey(w => w.ProductId)  // WishlistItem'daki ProductId, Product tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Restrict);  // Product silindiğinde, WishlistItem'lar etkilenmez

            // CartItem ile ApplicationUser ilişkisini yapılandırır
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.ApplicationUser)  // Her CartItem bir ApplicationUser'a sahiptir
                .WithMany(u => u.CartItems)  // Her ApplicationUser'un birçok CartItem'ı olabilir
                .HasForeignKey(c => c.ApplicationUserId)  // CartItem'daki ApplicationUserId, ApplicationUser tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // ApplicationUser silindiğinde ilişkili CartItem'lar da silinir

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)  // Her CartItem bir Product'a sahiptir
                .WithMany()  // Product modelinde CartItems koleksiyonu olmayabilir
                .HasForeignKey(c => c.ProductId)  // CartItem'daki ProductId, Product tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Restrict);  // Product silindiğinde, CartItem'lar etkilenmez

            // Notification ile ApplicationUser ilişkisini yapılandırır
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.ApplicationUser)  // Her Notification bir ApplicationUser'a sahiptir
                .WithMany(u => u.Notifications)  // Her ApplicationUser'un birçok Notification'ı olabilir
                .HasForeignKey(n => n.ApplicationUserId)  // Notification'daki ApplicationUserId, ApplicationUser tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // ApplicationUser silindiğinde ilişkili Notification'lar da silinir

        }
    }
}
