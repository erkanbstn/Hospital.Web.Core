using HospitalWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("HospitalConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/GirisYap"; // Giri� yapmadan bir sayfaya gidildi�inde y�nlendirilecek yol
        options.LogoutPath = "/Auth/CikisYap"; // ��k�� yap�ld���nda y�nlendirilecek yol
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.EnsureDatabaseCreated(); // Veritaban�n� yoksa olu�tur
    DbInitializer.Seed(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // Kimlik do�rulama
app.UseAuthorization();   // Yetkilendirme
app.MapStaticAssets();
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area=}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=GirisYap}/{id?}");
app.Run();
