using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShob.Domain.Entities;
namespace BookShob.Infrastructure.Configurations
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.catName);

            builder.Property(c => c.catName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.catOrder)
                   .IsRequired();

            builder.Ignore(c => c.createdDate);

            builder.Property(c => c.markedAsDeleted)
                   .HasColumnName("isDeleted");

            var seedData = new List<Category>();
            for (int i = 1; i <= 20; i++)
            {
                seedData.Add(new Category
                {
                    Id = i,
                    catName = $"Category {i}",
                    catOrder = i,
                    markedAsDeleted = false
                });
            }

            builder.HasData(seedData);
        }
    }
}
