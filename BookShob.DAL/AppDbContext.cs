using Microsoft.AspNetCore.Identity;
using BookShob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BookShob.Infrastructure
{
    public class AppDbContext:IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.FullName).HasMaxLength(200);
                b.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
