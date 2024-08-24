using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class OrderDetail :IEntity
    {
        [Key]
        public int OrderId { get; set; } // Sipariş ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public int ProductOptionId { get; set; } // Ürün seçeneği ID'si
        public int Quantity { get; set; } // Miktar
        public decimal Price { get; set; } // Fiyat

        public Order Order { get; set; } // Sipariş
        public Product Product { get; set; } // Ürün
        public ProductOption ProductOption { get; set; } // Ürün seçeneği
    }
}
