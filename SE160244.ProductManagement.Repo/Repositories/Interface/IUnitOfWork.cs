using SE160244.ProductManagement.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE160244.ProductManagement.Repo.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Product> ProductRepository { get; }

        IGenericRepository<Category> CategoryRepository { get; }

        Task<int> CommitAsync();


    }
}
