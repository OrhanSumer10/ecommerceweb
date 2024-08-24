using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        [Key]
        public int ProductId { get; set; }

        // Ürünün adı
        public string Name { get; set; }

        // Ürünün açıklaması
        public string Description { get; set; }

        // Ürünün birim fiyatı
        public double Price { get; set; }

        // Ürünün mevcut stoğu
        public int Stock { get; set; }

        // Ürünün resim URL'si
        public string ImageUrl { get; set; }

   

        // Ürünün SKU (Stock Keeping Unit) numarası
        public string SKU { get; set; }

        // Ürünün marka adı
        public string Brand { get; set; }

        // Ürünün üretim tarihi
        public DateTime ManufactureDate { get; set; }

        // Ürünün garanti süresi (gün cinsinden)
        public int WarrantyPeriod { get; set; }

        // Ürün puanı (ortalama kullanıcı puanı)
        public double Rating { get; set; }

        // Ürünün oluşturulma tarihi
        public DateTime CreatedDate { get; set; }

        // Ürünün güncellenme tarihi
        public DateTime UpdatedDate { get; set; }

        // Ürünün kategorisi
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // İlgili ürün detayları
        public ICollection<OrderDetail> OrderDetails { get; set; } // Sipariş detayları
        public ICollection<ProductReview> Reviews { get; set; } // Ürün incelemeleri

        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<ProductReview>();
        }
    }
}
