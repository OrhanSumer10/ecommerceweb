using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Order :IEntity
    {
        [Key]
        public int OrderId { get; set; } // Sipariş ID'si
        public int UserId { get; set; } // Kullanıcı ID'si
        public DateTime OrderDate { get; set; } // Sipariş tarihi
        public decimal TotalAmount { get; set; } // Toplam tutar
        public OrderStatus Status { get; set; } // Sipariş durumu
        public ApplicationUser User { get; set; } // Siparişin ait olduğu kullanıcı
        public ICollection<OrderDetail> OrderDetails { get; set; } // Sipariş detayları
        public Payment Payment { get; set; } // Ödeme bilgileri
        public int ShippingAddressId { get; set; } // Gönderim adresi ID'si
        public Address ShippingAddress { get; set; } // Gönderim adresi
        public int BillingAddressId { get; set; } // Fatura adresi ID'si
        public Address BillingAddress { get; set; } // Fatura adresi
        public ICollection<Coupon> AppliedCoupons { get; set; } // Kullanılan kuponlar

        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            AppliedCoupons = new HashSet<Coupon>();
        }
        public enum OrderStatus
        {
            Pending,        // Sipariş alındı, ancak henüz işleme alınmadı
            Processing,     // Sipariş işleniyor
            Shipped,        // Sipariş gönderildi
            Delivered,      // Sipariş teslim edildi
            Cancelled,      // Sipariş iptal edildi
            Returned        // Sipariş iade edildi
        }
    }
}
