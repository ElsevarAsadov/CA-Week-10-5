using Pustok.Models;

namespace Pustok.Business.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(Genre entity);
        Task Delete(int id);
        Task<Genre> GetByIdAsync(int id);
        Task<List<Genre>> GetAllAsync();
        Task UpdateAsync(Genre genre);
    }
}
