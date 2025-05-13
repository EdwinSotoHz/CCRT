/************* Entity Framework packages ***************/
using Microsoft.EntityFrameworkCore;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/************* Database Context ***************/
builder.Services.AddDbContext<CentroCulturalRegionalTlahuelilpanContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLString")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
