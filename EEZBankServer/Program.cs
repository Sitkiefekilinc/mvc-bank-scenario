using EEZBankServer.EfCore;
using EEZBankServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<UserDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IEEZBankUserService, EEZBankUserService>();
builder.Services.AddHttpClient<IDovizService, DovizService>(
    client => client.BaseAddress = new Uri("https://api.frankfurter.app/"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Home/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BireyselOnly", policy => policy.RequireClaim("UserType", "Bireysel"));
    options.AddPolicy("KurumsalOnly", policy => policy.RequireClaim("UserType", "Kurumsal"));
    options.AddPolicy("TicariOnly", policy => policy.RequireClaim("UserType", "Ticari"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("UserType", "Admin"));
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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.Run();
