using BookShob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShob.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task Update(Category category);
        Task Delete(Category category);

    }
}
