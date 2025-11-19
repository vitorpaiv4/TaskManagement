using System.Collections.Generic;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        Product Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
