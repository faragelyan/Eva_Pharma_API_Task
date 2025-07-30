using BookShob.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookShob.Infrastructure.Configurations
{
    internal class ProductConfig: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.Author)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnName("BookPrice");

            builder.Property(p => p.markedAsDeleted)
                   .HasColumnName("isDeleted");

            builder.HasOne(c => c.category)
                .WithMany(p => p.products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            var products = new List<Product>();
            for (int i = 1; i <= 20; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Title = $"Book {i}",
                    Description = $"This is the description of Book {i}",
                    Author = $"Author {i}",
                    Price = 10.0m * i,
                    CategoryId = i,
                    markedAsDeleted = false
                });
            }

            builder.HasData(products);
        }


    }
}
