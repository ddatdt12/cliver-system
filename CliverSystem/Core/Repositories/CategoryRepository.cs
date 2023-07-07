using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace CliverSystem.Core.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {

        }

        public async Task<IEnumerable<Subcategory>> GetPopularSubcategories()
        {
            return await _context.Subcategories.AsNoTracking().OrderByDescending(sC => sC.Posts.Count()).Take(10).ToListAsync();
        }
    }
}
