using Business.Abstract;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommorceWeb.ViewComponents
{
    public class CartItem : ViewComponent
    {
        private readonly IApplicationuserService _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly ICartService _cartService; // Sepet hizmetlerini yöneten servis
        private readonly IProductService _productService; // Ürün hizmetlerini yöneten servis
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartItem(IApplicationuserService userService, ICartService cartService, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            // Constructor içinde bağımlılıkları enjekte et
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
          

            var resultCart = _cartService.GetList();
            var allCartItems = resultCart.Data;

            var userCartItems = allCartItems.Where(ci => ci.ApplicationUserId == Convert.ToInt32(userId)).ToList();
            var productIds = userCartItems.Select(ci => ci.ProductId).Distinct().ToList();

            List<Product> products = new List<Product>();
            foreach (var productId in productIds)
            {
                var resultProduct = _productService.GetById(productId);
                if (resultProduct.Success)
                {
                    products.Add(resultProduct.Data);
                }
            }

            foreach (var item in userCartItems)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    item.Product.SKU = product.SKU;
                    item.Product.Name = product.Name;
                    item.Product.Description = product.Description;
                    item.Product.Price = product.Price;
                    item.Product.Brand = product.Brand;
                    item.Product.ImageUrl = product.ImageUrl;
                    item.Product.Rating = product.Rating;
                }
            }

            var model = new CartListViewModel
            {
                CartItems = userCartItems
            };

            return View( model); // "Default" ViewComponent kısmi görünümünün adı
        }
    }
    
}
