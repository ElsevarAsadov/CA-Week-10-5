using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Data.Repositories.Implementations
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(PustokContext context) : base(context)
        {
        }
    }
}
