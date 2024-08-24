using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; } // Bildirim ID'si
        public int ApplicationUserId { get; set; } // Kullanıcı ID'si
        public string Content { get; set; } // Bildirim içeriği
        public DateTime CreatedAt { get; set; } // Bildirimin oluşturulma tarihi
        public bool IsRead { get; set; } // Bildirimin okunup okunmadığını belirten flag

        // Navigation properties
        public ApplicationUser ApplicationUser { get; set; } // Kullanıcı
    }

}
