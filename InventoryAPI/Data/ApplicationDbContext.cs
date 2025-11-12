using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Data
{
  /// <summary>
  /// Entity Framework Core database context for the Inventory API.
  /// </summary>
  /// <remarks>
  /// Exposes entity sets and configures model relationships, delete behaviors, and seed data.
  /// </remarks>
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Unsets representing the entities in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Role entities in the database.
    /// </summary>
    public DbSet<Role> Roles { get; set; }
    /// <summary>
    /// Category entities in the database.
    /// </summary>
    public DbSet<Category> Categories { get; set; }
    /// <summary>
    /// Product entities in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Configures the EF Core model.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
                 .HasOne(u => u.Role)
                 .WithMany(r => r.Users)
                 .HasForeignKey(u => u.RoleId)
                 .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Product>()
          .HasOne(p => p.Category)
          .WithMany(c => c.Products)
          .HasForeignKey(p => p.CategoryId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<Product>()
          .HasOne(p => p.CreatedByUser)
          .WithMany()
          .HasForeignKey(p => p.CreatedByUserId)
          .OnDelete(DeleteBehavior.Restrict);


      ///Seed data

      modelBuilder.Entity<Role>().HasData(
          new Role { Id = "1", Name = "Administrator" },
          new Role { Id = "2", Name = "Operator" },
          new Role { Id = "3", Name = "Viewer" }
      );

      CreatePasswordHash("admin123", out byte[] passwordHash, out byte[] passwordSalt);
      CreatePasswordHash("operator123", out byte[] operatorHash, out byte[] operatorSalt);
      CreatePasswordHash("viewer123", out byte[] viewerHash, out byte[] viewerSalt);
      CreatePasswordHash("12345", out byte[] tempHash, out byte[] tempSalt);
      modelBuilder.Entity<User>().HasData(
          new User
          {
            Id = 1,
            Username = "admin",
            Email = "admin@gmail.com",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            RoleId = "1",
          },
          new User
          {
            Id = 2,
            Username = "operator",
            Email = "operator@gmail.com",
            PasswordHash = operatorHash,
            PasswordSalt = operatorSalt,
            RoleId = "2",
          },
          new User
          {
            Id = 3,
            Username = "diegoleon",
            Email = "diegoleon@gmail.com",
            PasswordHash = tempHash,
            PasswordSalt = tempSalt,
            RoleId = "3",
          },
          new User
          {
            Id = 4,
            Username = "viewer",
            Email = "viewer@gmail.com"
            ,
            PasswordHash = viewerHash,
            PasswordSalt = viewerSalt,
            RoleId = "3",
          },
          new User
          {
            Id = 5,
            Username = "yenifer",
            Email = "yenifer@gmail.com",
            PasswordHash = tempHash,
            PasswordSalt = tempSalt,
            RoleId = "1",
          }


      );

      modelBuilder.Entity<Category>().HasData(
          new Category { Id = 1, Name = "Electronics" },
          new Category { Id = 2, Name = "Clothing" },
          new Category { Id = 3, Name = "Home" },
          new Category { Id = 4, Name = "Sports" },
          new Category { Id = 5, Name = "Books" }
      );

      var currentTime = DateTime.UtcNow;
      modelBuilder.Entity<Product>().HasData(
               new Product
               {
                 Id = 1,
                 Name = "iPhone 17 Pro Max",
                 Description = "Latest Apple smartphone",
                 StockQuantity = 25,
                 Price = 999.99m,
                 PictureUrl = "https://co.tiendasishop.com/cdn/shop/files/IMG-18067850_m_jpeg_1_2a54582a-fa6e-45ad-a900-152fe5523272.jpg?v=1757539925&width=823",
                 CategoryId = 1,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 2,
                 Name = "Samsung Galaxy S25",
                 Description = "Android flagship phone with AI features",
                 StockQuantity = 30,
                 Price = 849.99m,
                 PictureUrl = "https://smselectronic.com/wp-content/uploads/2025/01/SAMSUNG-GALAXY-S25-ULTRA-512-GB-NEGRO-TITANIO.png",
                 CategoryId = 1,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 3,
                 Name = "MacBook Air M3",
                 Description = "Lightweight laptop for professionals",
                 StockQuantity = 15,
                 Price = 1299.99m,
                 PictureUrl = "https://mac-center.com/cdn/shop/files/IMG-12444373.jpg?v=1723750058&width=823",
                 CategoryId = 1,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 4,
                 Name = "SAMSUNG Gamer 49 Pulgadas Odyssey",
                 Description = "Get your head in the game with the 49 inch Odyssey G9",
                 StockQuantity = 40,
                 Price = 349.99m,
                 PictureUrl = "https://images.samsung.com/is/image/samsung/p6pim/co/ls49cg950enxza/gallery/co-odyssey-g9-g95c-ls49cg950enxza-542518742",
                 CategoryId = 1,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },

               new Product
               {
                 Id = 5,
                 Name = "Nike Air Max 270",
                 Description = "Comfortable running shoes",
                 StockQuantity = 50,
                 Price = 149.99m,
                 PictureUrl = "https://static.nike.com/a/images/t_web_pdp_936_v2/f_auto/awjogtdnqxniqqk0wpgf/AIR+MAX+270.png",
                 CategoryId = 2,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 6,
                 Name = "Levi's 501 Jeans",
                 Description = "Classic straight-fit jeans",
                 StockQuantity = 35,
                 Price = 79.99m,
                 PictureUrl = "https://lsco.vtexassets.com/arquivos/ids/325320-800-auto?v=638941623505170000&width=800&height=auto&aspect=true",
                 CategoryId = 2,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 7,
                 Name = "Adidas Essentials Fleece Hoodie",
                 Description = "Warm and comfortable hoodie",
                 StockQuantity = 20,
                 Price = 59.99m,
                 PictureUrl = "https://cdn.media.amplience.net/i/frasersdev/53107203_o?fmt=auto&upscale=false&w=767&h=767&sm=scaleFit&$h-ttl$",
                 CategoryId = 2,
                  CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },

               new Product
               {
                 Id = 8,
                 Name = "Dyson V11 Vacuum",
                 Description = "Cordless stick vacuum cleaner",
                 StockQuantity = 12,
                 Price = 599.99m,
                 PictureUrl = "https://www.hardysofkilkeel.co.uk/media/catalog/product/cache/4ae023eae5c6c134dba0f643c3f0818b/d/y/dyson_v11-advance_1157048_34-0100-0172.jpg",
                 CategoryId = 3,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 9,
                 Name = "Instant Pot Duo",
                 Description = "7-in-1 multi-functional pressure cooker",
                 StockQuantity = 28,
                 Price = 89.99m,
                 PictureUrl = "https://instantpot.com/cdn/shop/files/IB_112-0170-01_113-0059-01_Duo-V5-6QT-8QT_ATF_Square_Tile1_1.png?v=1745807645",
                  CategoryId = 3,
                  CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },

               new Product
               {
                 Id = 10,
                 Name = "Yoga Mat Premium",
                 Description = "Non-slip exercise mat",
                 StockQuantity = 60,
                 Price = 29.99m,
                 PictureUrl = "https://cdn.thewirecutter.com/wp-content/media/2024/07/yoga-mat-2048px-1629-3x2-1.jpg?auto=webp&quality=75&crop=16:9,smart&width=1024",
                 CategoryId = 4,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 11,
                 Name = "Basketball Official Size 7",
                 Description = "Professional basketball",
                 StockQuantity = 45,
                 Price = 49.99m,
                 PictureUrl = "https://cdn11.bigcommerce.com/s-nehqfptb/images/stencil/1280x1280/products/39768/76435/Baden-Deluxe-Rubber-Basketball-Size-7-Front-View__88663.1702133690.jpg?c=2",
                 CategoryId = 4,
                  CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },

               new Product
               {
                 Id = 12,
                 Name = "The Widow: A Novel",
                 Description = "John Grisham is the acclaimed master of the legal thriller. Now, he’s back with his first-ever whodunit",
                 StockQuantity = 100,
                 Price = 34.99m,
                 PictureUrl = "https://bjs.scene7.com/is/image/bjs/345379?$bjs-Initial600$",
                 CategoryId = 5,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 13,
                 Name = "The Housemaid",
                 Description = "Audible Audiobook – Unabridged",
                 StockQuantity = 75,
                 Price = 54.99m,
                 PictureUrl = "https://i5.walmartimages.com/seo/The-Housemaid-Paperback-9781538768549_f5600821-60ac-4060-bd96-b3601c353880.19a88944fcc2cf3a1c33a1c82be666d9.jpeg",
                 CategoryId = 5,
                 CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               },
               new Product
               {
                 Id = 14,
                 Name = "One Hundred Years of Solitude",
                 Description = "One of the most influential literary works of our time",
                 StockQuantity = 30,
                 Price = 44.99m,
                 PictureUrl = "https://librerialerner.vtexassets.com/arquivos/ids/2407562-800-800?v=638892677599870000&width=800&height=800&aspect=true",
                 CategoryId = 5,
                  CreatedByUserId = 1,
                 CreatedAt = currentTime,
                 LastUpdatedAt = currentTime
               }
           );

      // Call base after custom configuration and seeding.
      base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Private helper method to create password hash and salt.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <param name="passwordSalt"></param>
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}