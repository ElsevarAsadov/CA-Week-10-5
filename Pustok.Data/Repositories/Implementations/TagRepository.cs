using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Data.Repositories.Implementations
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(PustokContext context) : base(context)
        {
        }
    }
}
