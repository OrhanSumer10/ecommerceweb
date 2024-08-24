using Core.Utilities.Results;
using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
