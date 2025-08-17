using Microsoft.EntityFrameworkCore;

namespace BonnyBabyStore.Models
{

        public class ApplicationDbContext : DbContext
        {

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {

            }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
         
        public DbSet<Product> Products { get; set; }
        }


    }



