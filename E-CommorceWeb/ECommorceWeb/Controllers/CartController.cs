using Business.Abstract;
using Core.Utilities.Results;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommorceWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IApplicationuserService _userService; // Kullanıcı hizmetlerini yöneten servis
        private readonly ICartService _cartService; // Sepet hizmetlerini yöneten servis
        private readonly IProductService _productService; // Ürün hizmetlerini yöneten servis

        public CartController(IApplicationuserService userService, ICartService cartService, IProductService productService)
        {
            // Constructor içinde bağımlılıkları enjekte et
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            // Giriş yapmış kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                // Kullanıcı giriş yapmamışsa, giriş sayfasına yönlendir
                return RedirectToAction("Index", "Login");
            }

            // Tüm sepet öğelerini al
            var resultCart = _cartService.GetList();
            List<CartItem> allCartItems = resultCart.Data;

            // Giriş yapan kullanıcının sepet öğelerini filtrele
            var userCartItems = allCartItems.Where(ci => ci.ApplicationUserId == Convert.ToInt32(userId)).ToList();
           ViewBag.userCartItems = userCartItems;
            // Farklı ürün ID'lerini al
            var productIds = userCartItems.Select(ci => ci.ProductId).Distinct().ToList(); // Benzersiz ürün ID'lerini listeye al

            // Her bir benzersiz ürün ID'si için ürün bilgilerini al
            List<Product> products = new List<Product>();
            foreach (var productId in productIds)
            {
                var resultProduct = _productService.GetById(productId);
                if (resultProduct.Success) // Ürün başarılı bir şekilde alındıysa
                {
                    products.Add(resultProduct.Data); // Ürünü listeye ekle
                }
            }

            // Sepet öğelerini güncelle
            foreach (var item in userCartItems)
            {
                var product = products.FirstOrDefault(p => p.ProductId == item.ProductId); // Ürün bilgilerini eşleştir
                if (product != null)
                {
                    // Sepet öğesinin ürün bilgilerini güncelle
                    item.Product.SKU = product.SKU;
                    item.Product.Name = product.Name;
                    item.Product.Description = product.Description;
                    item.Product.Price = product.Price;
                    item.Product.Brand = product.Brand;
                    item.Product.ImageUrl = product.ImageUrl;
                    item.Product.Rating = product.Rating;
                }
            }

            // ViewModel'i oluştur
            var model = new CartListViewModel
            {
                CartItems = userCartItems // Sepet öğelerini modele ekle
            };
            
            return View(model); // View'a modeli gönder
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // Kullanıcının kimliğini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa giriş sayfasına yönlendir
            }

            // Ürünü al
            var product = _productService.GetById(productId).Data;
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı."; // Ürün yoksa hata mesajı göster
                return RedirectToAction("Index", "Product"); // Ürün sayfasına yönlendir
            }

            // Sepetteki mevcut ürünleri al
            var existingCartItem = _cartService.GetListByProduct(productId).Data
                .FirstOrDefault(ci => ci.ApplicationUserId == Convert.ToInt32(userId));
            if (existingCartItem != null)
            {

                // Eğer sepetteki ürün varsa, miktarı artır
                if (existingCartItem.ProductId == productId)
                { 
                    existingCartItem.Quantity += 1;
                    existingCartItem.Product = product;

                    try
                    {
                        // Sepet öğesini güncelle
                        var updateResult = _cartService.Update(existingCartItem); // Sepeti güncelle

                        if (updateResult.Success)
                        {
                            TempData["Message"] = "Ürün Sepete Eklendi."; // Başarılı güncelleme mesajı
                        }
                        else
                        {
                            TempData["ErrorMessage"] = updateResult.Message; // Hata mesajı göster
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        TempData["ErrorMessage"] = "Güncelleme sırasında bir hata oluştu: " + ex.InnerException?.Message; // Hata mesajı göster
                    }

                    return RedirectToAction("Index", "Product"); // Sepet sayfasına dön
                }
            }


            // Eğer sepetteki ürün yoksa, yeni ürün ekle
            var newCartItem = new CartItem
            {
                ProductId = productId,
                ApplicationUserId = Convert.ToInt32(userId),
                Quantity = 1, // Varsayılan miktar
                AddedDate = DateTime.Now, // Eklenme tarihi
            };

            var addResult = _cartService.Add(newCartItem); // Yeni ürünü sepete ekle
            if (addResult.Success)
            {
                TempData["Message"] = "Ürün sepete eklendi."; // Başarılı ekleme mesajı
            }
            else
            {
                TempData["ErrorMessage"] = addResult.Message; // Hata mesajı göster
            }

            return RedirectToAction("Index", "Product"); // Ürün sayfasına geri dön
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa giriş sayfasına yönlendir
            }

            var cartItem = _cartService.GetById(cartItemId).Data; // Sepet öğesini al
                                                                  // Ürünü al
            var product = _productService.GetList().Data;
            cartItem.Product = product.FirstOrDefault();
            if (cartItem == null)
            {
                TempData["ErrorMessage"] = "Sepet öğesi bulunamadı."; // Hata mesajı göster
                return RedirectToAction("Index", "Cart"); // Sepet sayfasına dön
            }
            var result = _cartService.Delete(cartItem); // Sepet öğesini sil

            if (result.Success)
            {
                TempData["Message"] = result.Message; // Başarılı silme mesajı
            }
            else
            {
                TempData["ErrorMessage"] = result.Message; // Hata mesajı göster
            }

            return RedirectToAction("Index", "Cart"); // Sepet sayfasına geri dön
        }
    }
}
