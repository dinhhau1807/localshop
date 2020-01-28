using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using localshop.Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace localshop.Domain.Concretes
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("LocalShopConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #region DbSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Status> Statuses { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //------------------------------------------------------------------------
            // Products
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Sku).IsRequired().HasMaxLength(450);
            modelBuilder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.MetaTitle).IsRequired();

            //------------------------------------------------------------------------
            // Images
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            modelBuilder.Entity<Image>().Property(i => i.Path).IsRequired();
            modelBuilder.Entity<Image>().Property(i => i.ProductId).IsRequired();

            //------------------------------------------------------------------------
            // Categories
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();

            //------------------------------------------------------------------------
            // Statuses
            modelBuilder.Entity<Status>().HasKey(s => s.Id);
            modelBuilder.Entity<Status>().Property(s => s.Name).IsRequired();



            base.OnModelCreating(modelBuilder);
        }
    }
}
