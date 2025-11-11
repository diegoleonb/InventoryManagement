using InventoryAPI.Data;
using InventoryAPI.Interfaces;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  options.UseSqlServer(connectionString);
});

// Dependency Injection Configuration
ConfigureServices(builder.Services);

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
              .GetBytes(builder.Configuration["Jwt:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      };
    });

// Authorization
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Inventory API",
    Version = "v1",
    Description = "API for inventory management with role-based access control"
  });

  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngular", policy =>
  {
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// Create database if not exists using EnsureCreatedAsync()
await InitializeDatabase(app);

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Helper method to configure services
void ConfigureServices(IServiceCollection services)
{
  // Repositories
  services.AddScoped<IUserRepository, UserRepository>();
  services.AddScoped<IRoleRepository, RoleRepository>();
  services.AddScoped<ICategoryRepository, CategoryRepository>();
  services.AddScoped<IProductRepository, ProductRepository>();

  // Services
  services.AddScoped<IAuthService, AuthService>();
  services.AddScoped<IProductService, ProductService>();
  services.AddScoped<ICategoryService, CategoryService>();
}

// Helper method to initialize database using EnsureCreatedAsync()
async Task InitializeDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;
  try
  {
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Ensure database is created asynchronously and seed data
    var created = await context.Database.EnsureCreatedAsync();

    if (created)
    {
      var logger = services.GetRequiredService<ILogger<Program>>();
      logger.LogInformation("Database created successfully");
    }
    else
    {
      Console.WriteLine("Database already exists");
    }
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while creating the database");
  }
}