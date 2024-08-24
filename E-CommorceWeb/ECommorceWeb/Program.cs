using Business.Abstract;
using Business.Concrete;
using DataAcsess.Abstract;
using DataAcsess.Concrete.EntityFramework;
using DataAcsess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddSingleton<IProductService, ProductManager>();
builder.Services.AddSingleton<IProductDal, EfProductDal>();
builder.Services.AddSingleton<ICategroyService, CategoryManager>();
builder.Services.AddSingleton<ICategoryDal, EfCategoryDal>();
builder.Services.AddSingleton<ILoginService, LoginManager>();
builder.Services.AddSingleton<ILoginDal, EfLoginDal>();
builder.Services.AddSingleton<ICartService, CartManager>();
builder.Services.AddSingleton<ICartDal, EfCartDal>();
builder.Services.AddSingleton<IApplicationuserService, ApplicationuserManager>();
builder.Services.AddSingleton<IApplicationuserDal, EfApplicationuserDal>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
         {
             options.LoginPath = "/Login/Index";
             options.LogoutPath = "/Login/Logout";
             options.AccessDeniedPath = "/Login/AccessDenied";
         });


builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication(); // Bu satýrý kontrol edin
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
