using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShob.Application.Interfaces;
namespace BookShob.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public UnitOfWork(AppDbContext context,
                          ICategoryRepository categoryRepository,
                          IProductRepository productRepository)
        {
            this.context = context;
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }


    }
}
