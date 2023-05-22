using admin_web_sell_phone.DataAccess;
using admin_web_sell_phone.Service;
using admin_web_sell_phone.Service.implamentation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Nhanvien/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISanphamService, SanphamService>();
builder.Services.AddScoped<IThuonghieuService, ThuonghieuService>();
builder.Services.AddScoped<IKhachhangService, KhachhangService>();
builder.Services.AddScoped<INhanvienService,NhanvienService>();
builder.Services.AddScoped <INhacungcapService, NhacungcapService>();
builder.Services.AddScoped <IPhieunhapService, PhieunhapService>();
builder.Services.AddScoped<IChitietphieunhapService, ChitietphieunhapService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IChitiethoadonService, ChitiethoadonService>();
builder.Services.AddSession();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  //  app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Nhanvien}/{action=Login}/{id?}");

app.Run();
