using Microsoft.EntityFrameworkCore;
using RezerwacjaHoteli.Data;
using RezerwacjaHoteli.Models;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HotelReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelReservationDbContext>();
    context.Database.Migrate();
    SeedData(context);
}

app.Run();

void SeedData(HotelReservationDbContext context)
{
    if (context.Users.Any())
    {
        return;
    }

    // Testowi użytkownicy
    var adminPassword = HashPassword("Admin123!");
    var guestPassword = HashPassword("Guest123!");

    var users = new List<User>
    {
        new User
        {
            Email = "admin@hotel.com",
            PasswordHash = adminPassword,
            FullName = "Admin",
            PhoneNumber = "123456789",
            Role = "Admin",
            CreatedDate = DateTime.Now
        },
        new User
        {
            Email = "guest@example.com",
            PasswordHash = guestPassword,
            FullName = "Guest",
            PhoneNumber = "987654321",
            Role = "Guest",
            CreatedDate = DateTime.Now
        }
    };

    context.Users.AddRange(users);
    context.SaveChanges();

    var hotels = new List<Hotel>
    {
        new Hotel
        {
            Name = "Grand Warsaw Hotel",
            Address = "ul. Żwirki i Wigury 16A",
            City = "Warszawa",
            StarRating = 5,
            Description = "Luksusowy hotel w centrum miasta",
            ContactEmail = "info@grand.com",
            IsActive = true,
            CreatedDate = DateTime.Now
        },
        new Hotel
        {
            Name = "Grand Krakow Hotel",
            Address = "ul. Kapelanka 42B",
            City = "Kraków",
            StarRating = 4,
            Description = "Hotel idealny na spotkania biznesowe",
            ContactEmail = "info@grand.com",
            IsActive = true,
            CreatedDate = DateTime.Now
        }
    };

    context.Hotels.AddRange(hotels);
    context.SaveChanges();
}

string HashPassword(string password)
{
    using (var sha256 = System.Security.Cryptography.SHA256.Create())
    {
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}
