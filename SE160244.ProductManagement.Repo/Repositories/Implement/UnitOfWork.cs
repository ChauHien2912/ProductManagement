using SE160244.ProductManagement.Repo.Models;
using SE160244.ProductManagement.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE160244.ProductManagement.Repo.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly MyStoreDBContext _context;
        private IGenericRepository<Product> productRepository;
        private IGenericRepository<Category> categoryRepository;



        public UnitOfWork()
        {
            _context = new MyStoreDBContext();
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(_context);
                }
                return categoryRepository;
            }
        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
