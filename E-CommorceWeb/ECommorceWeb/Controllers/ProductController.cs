using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAcsess.Concrete.EntityFramework.Contexts;
using ECommorceWeb.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using System.Security.Claims;

namespace ECommorceWeb.Controllers
{

    public class ProductController : Controller
    {
        IProductService _productService;
        ICategroyService _categoryService;

        public ProductController(IProductService productService, ICategroyService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
       
        public IActionResult Index()
        {
            // Kategorileri al
            var categories = _categoryService.GetList();

            // Kategori listesini SelectListItem listesine dönüştür
            List<SelectListItem> categoryValue = categories.Data.Select(c => new SelectListItem
            {
                Text = c.Name, // Kategori adı
                Value = c.CategoryId.ToString() // Kategori ID'si
            }).ToList();

            // ViewBag'e atama yap
            ViewBag.cv = categoryValue;


            var result = _productService.GetList();
            // Ürünleri al
            List<Product> products = result.Data;

            // Modeli oluştur
            var viewModel = new ProductListViewModel
            {
                Products = products

            };
            if (result.Success)
            {
                // Modeli View'a gönder

                return View(viewModel);
            }
            // Hata durumunu işleyin
            return BadRequest(result.Message);

        }
        [Authorize(Roles = "Admin")]//claim olarak eklediğimiz isadmin değerine göre ve iki seçenek kullanmak istiyorsak araya virgül atarak 
        public IActionResult AdminIndex()
        {
            // Kategorileri al
            var categories = _categoryService.GetList();

            // Kategori listesini SelectListItem listesine dönüştür
            List<SelectListItem> categoryValue = categories.Data.Select(c => new SelectListItem
            {
                Text = c.Name, // Kategori adı
                Value = c.CategoryId.ToString() // Kategori ID'si
            }).ToList();

        // ViewBag'e atama yap
        ViewBag.cv = categoryValue;


            var result = _productService.GetList();
        // Ürünleri al
        List<Product> products = result.Data;

        // Modeli oluştur
        var viewModel = new ProductListViewModel
        {
            Products = products

        };
            if (result.Success)
            {
                // Modeli View'a gönder

                return View(viewModel);
    }
            // Hata durumunu işleyin
            return BadRequest(result.Message);
}
    }
}
