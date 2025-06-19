using Employee.BusinessLogic;
using Employee.DataAccess.DependencyInjector;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.UI.DIConfiguration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*builder.WebHost.UseUrls("http://*:80");*/

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = PathString.FromUriComponent("/Home/Index");
        options.AccessDeniedPath = PathString.FromUriComponent("/Employees/AccessDenied");
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.
    AddBusinessLogic()
    .AddDataAccess(builder.Configuration)
    .AddSessionConfiguration()
    .AddRateLimiter()
    .AddCompressionConfiguration()
    .AddDbContextConfiguraion(builder.Configuration);


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

app.UseSessionconfiguration();
app.UseResponseCompression();
app.UseRateLimiters();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

// Middleware Order
// CORS ( Cross Origin Requests )
// Use Solid for the ASP MVC 
// Repository pattern 
// CRUD multiple controleller for performance
// ViewModel or Dto is best or performance

// Dont use Db calls in a loop ( N + 1 ) issue

