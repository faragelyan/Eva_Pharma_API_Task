using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShob.Application.Interfaces;
using BookShob.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BookShob.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task AddAsync(Product product)
        {
            await context.Products.AddAsync(product);
        }
        public Task Update(Product product)
        {
            context.Products.Update(product);
            return Task.CompletedTask;  
        }
        public Task Delete(Product product)
        {
            product.markedAsDeleted = true;
            context.Products.Update(product);
            return Task.CompletedTask;
        }

    }
}
