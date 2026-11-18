using Microsoft.EntityFrameworkCore;
using HealthcareApi.Data;
using HealthcareApi.Models;



var builder = WebApplication.CreateBuilder(args);

// CORS Policy Added
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite React default port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Database Context Registration ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// -------------------------------------

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHostedService<HealthcareApi.Services.ExpiredLockCleanupService>();
// Native .NET OpenAPI support (.NET 9/10 standard)
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS Middleware (Must be before Authorization and MapControllers)
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

// Program.cs ke andarr sabse niche yeh daalna hai:

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Id = 1, Name = "Nikesh (Patient)", Email = "patient@medislot.com", PasswordHash = "123456", Role = "Patient" },
            new User { Id = 2, Name = "Dr. Sharma", Email = "doctor@medislot.com", PasswordHash = "123456", Role = "Doctor" }
        );
        context.SaveChanges();
    }
}

app.Run(); // Yeh line pehle se hogi, bas iske upar block rakhna hai