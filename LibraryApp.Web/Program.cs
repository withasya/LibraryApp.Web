using LibraryApp.Web.Data;
using AutoMapper;
using LibraryApp.Web.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper'ı DI container'a ekleyin
builder.Services.AddAutoMapper(typeof(MappingProfile)); // Profilin bulunduğu türü ekliyoruz

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Geliştirme ortamında hata ayıklama sayfasını kullan
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Özel rota tanımı ekleyin
app.MapControllerRoute(
    name: "books",
    pattern: "api/books/{action=GetAllBooks}/{id?}",
    defaults: new { controller = "Books" });

app.Run();
