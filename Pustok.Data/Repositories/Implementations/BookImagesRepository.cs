using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Data.Repositories.Implementations
{
    public class BookImagesRepository : GenericRepository<BookImage>, IBookImagesRepository
    {
        public BookImagesRepository(PustokContext context) : base(context)
        {
        }
    }
}
