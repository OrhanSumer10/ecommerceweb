using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category :IEntity
    {
        [Key]
        public int CategoryId { get; set; } // Kategori ID'si
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması
        public int? ParentCategoryId { get; set; } // Üst kategori ID'si
        public Category ParentCategory { get; set; } // Üst kategori
        public ICollection<Category> SubCategories { get; set; } // Alt kategoriler
        public ICollection<Product> Products { get; set; } // Ürünler

        public Category()
        {
            SubCategories = new HashSet<Category>();
            Products = new HashSet<Product>();
        }
    }
}
