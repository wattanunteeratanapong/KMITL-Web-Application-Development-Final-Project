using Microsoft.EntityFrameworkCore;
using Togeta.Data;
using Togeta.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllersWithViews();

// Register the ApplicationDBContext with the DI container
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services (e.g., IUserService and its implementation)
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Middleware configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files

app.UseRouting();
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
