using Microsoft.EntityFrameworkCore;
using WebMvcApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ---- Registro de los DOS contextos (uno por cada contenedor) ----
builder.Services.AddDbContext<SqlServerContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

var mariaConn = builder.Configuration.GetConnectionString("MariaDb")!;
builder.Services.AddDbContext<MariaDbContext>(opt =>
    opt.UseMySql(mariaConn, ServerVersion.AutoDetect(mariaConn)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
