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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Category>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            return await context.Categories
                .Where(c => !c.markedAsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await context.Categories.CountAsync(c => !c.markedAsDeleted);
        }
        public async Task<List<Category>> GetAllAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Category category)
        {
            await context.Categories.AddAsync(category);
        }

        public Task Update(Category category)
        {
             context.Categories.Update(category);
            return Task.CompletedTask;
        }
        public Task Delete(Category category)
        {
            category.markedAsDeleted = true;
            context.Categories.Update(category);
            return Task.CompletedTask;
        }

    }
}
