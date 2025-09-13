
using BookStore.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. ...",
                    ISBN = "SWD9999001",
                    ListPrice = 99.00,
                    Price = 90.00,
                    Price50 = 85.00,
                    Price100 = 80.00,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. ...",
                    ISBN = "CAW777777701",
                    ListPrice = 40.00,
                    Price = 30.00,
                    Price50 = 25.00,
                    Price100 = 20.00,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. ...",
                    ISBN = "RITO5555501",
                    ListPrice = 55.00,
                    Price = 50.00,
                    Price50 = 40.00,
                    Price100 = 35.00,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. ...",
                    ISBN = "WS3333333301",
                    ListPrice = 70.00,
                    Price = 65.00,
                    Price50 = 60.00,
                    Price100 = 55.00,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. ...",
                    ISBN = "WS3333333301",
                    ListPrice = 70.00,
                    Price = 65.00,
                    Price50 = 60.00,
                    Price100 = 55.00,
                    CategoryId = 1
                }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Tech Solutions",
                    StreetAddress = "123 Tech Lane",
                    City = "Techville",
                    State = "TS",
                    PostalCode = "12345",
                    PhoneNumber = "123-456-7890"
                },
                new Company
                {
                    Id = 2,
                    Name = "Business Corp",
                    StreetAddress = "456 Business Rd",
                    City = "Businesstown",
                    State = "BC",
                    PostalCode = "67890",
                    PhoneNumber = "987-654-3210"
                },
                new Company
                {
                    Id = 3,
                    Name = "Enterprise Inc",
                    StreetAddress = "789 Enterprise Ave",
                    City = "Enterprisecity",
                    State = "EI",
                    PostalCode = "11223",
                    PhoneNumber = "555-555-5555"
                }
            );

        }
    }
}
