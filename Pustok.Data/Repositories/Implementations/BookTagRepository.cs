using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Data.Repositories.Implementations
{
    public class BookTagRepository : GenericRepository<BookTag>, IBookTagsRepository
    {
        public BookTagRepository(PustokContext context) : base(context)
        {
        }
    }
}
