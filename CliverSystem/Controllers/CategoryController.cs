using CliverSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CliverSystem.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult>Get()
        {
            var categories = await _unitOfWork.Categories.Find(includeProperties: "Subcategories").ToListAsync();
            return Ok(new
            {
                data = categories,
            });
        }

    }
}
