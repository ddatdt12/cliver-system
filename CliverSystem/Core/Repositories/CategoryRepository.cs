using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context, ILogger logger) : base(context, logger)
        {

        }


    }
}
