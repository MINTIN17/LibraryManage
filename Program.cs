
using Microsoft.Extensions.DependencyInjection;
using LibraryManage.Repository;
using LibraryManage.Service;
using LibraryManage.Service.Impl;
using LibraryManage.Context;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<LibraryItemRepository>();
builder.Services.AddTransient<ILibraryItemService, LibraryItemServiceImpl>();
builder.Services.AddTransient<BorrowerRepository>();
builder.Services.AddTransient<IBorrowerService, BorrowerServiceImpl>();
builder.Services.AddTransient<BorrowHistoryRepository>();
builder.Services.AddTransient<IBorrowHistoryService, BorrowHistoryServiceImpl>();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(x =>
{
    var cloudName = builder.Configuration["Cloudinary:CloudName"];
    var apiKey = builder.Configuration["Cloudinary:ApiKey"];
    var apiSecret = builder.Configuration["Cloudinary:ApiSecret"];

    var account = new Account(cloudName, apiKey, apiSecret);
    return new Cloudinary(account);
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LibraryItem}/{action=Index}/{id?}");

app.Run();
