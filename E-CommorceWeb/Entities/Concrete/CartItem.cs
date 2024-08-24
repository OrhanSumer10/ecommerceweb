using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    public class CartItem : IEntity
    {
        [Key]
        public int CartItemId { get; set; } // Cart item ID
        public int ApplicationUserId { get; set; } // Kullanıcı ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public int Quantity { get; set; } // Ürün miktarı
        public DateTime AddedDate { get; set; } // Ürünün ne zaman eklendiği

        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; } // Kullanıcı
        public Product Product { get; set; } = new Product(); // Ürün
    }

}
