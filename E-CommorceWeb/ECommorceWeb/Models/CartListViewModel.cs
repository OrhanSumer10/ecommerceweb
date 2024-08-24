

using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class CartListViewModel
    {
        public List<CartItem>  CartItems { get; set; }
        public List<ApplicationUser>  ApplicationUsers { get; set; }
    }
}
