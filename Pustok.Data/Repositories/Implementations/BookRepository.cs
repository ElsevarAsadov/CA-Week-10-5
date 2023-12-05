using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Data.Repositories.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(PustokContext context) : base(context)
        {
        }
    }
}
