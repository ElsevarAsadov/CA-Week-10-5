using Pustok.Models;

namespace Pustok.Business.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(Book entity);
        Task SoftDelete(int id);
        Task Delete(int id);
        Task<Book> GetByIdAsync(int id);
        Task<List<Book>> GetAllAsync();
        Task UpdateAsync(Book entity);
    }
}
