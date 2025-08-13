using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allow any origin (including 'null')
              .AllowAnyMethod()   // Allow all HTTP methods
              .AllowAnyHeader();  // Allow all headers
    });
});

// ✅ Add PostgreSQL DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"🔍 Connection String: {connectionString}");

// ✅ Add PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
);

var app = builder.Build();

//test if the connection works
using (var scope = app.Services.CreateScope()) // Create a scope to resolve services
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        await context.Database.OpenConnectionAsync(); // Open the connection
        Console.WriteLine("✅ Database connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database connection failed: {ex.Message}");
    }
}

// ✅ Use CORS middleware BEFORE routing
app.UseCors("AllowAll");

app.UseStaticFiles();


// ✅ Map API routes
app.MapGet("/complaint", async (AppDbContext db) =>
{
    try{
        var complaints = await db.Complaint!.ToListAsync();
        Console.WriteLine($"Retrieved: {complaints}");
        return Results.Ok(complaints);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500
        );
    } 
});

// map post complaint
app.MapPost("/complaint", async (AppDbContext db, Complaint complaint) =>
{
    try{
        db.Complaint!.Add(complaint);
        await db.SaveChangesAsync();
        return Results.Created($"/complaint/{complaint.Id}", complaint);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred: {ex.Message}");
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500
        );
    }
});

// ✅ IMPORTANT: Move fallback to the END (after API routes)
app.MapFallbackToFile("index.html");

// ✅ Run the app
app.Run();

//dotnet run --project WebApplication1.csproj
//running on https://localhost:7165 or http://localhost:5146



