using MiAppApi.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Configurar conexión a PostgreSQL para Neon
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        // Configuraciones específicas para Neon
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorCodesToAdd: null);

        // Timeout más bajo para Neon (es más rápido)
        npgsqlOptions.CommandTimeout(30);

        // Configuraciones específicas para conexiones serverless
        npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
    });

    // Solo en desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddControllers();

var app = builder.Build();

// Test de conexión inicial
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await context.Database.CanConnectAsync();
        Console.WriteLine("✅ Conexión a Neon exitosa");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error de conexión a Neon: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();