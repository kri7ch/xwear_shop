using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DbExwearContext _context;

        public CategoriesController(DbExwearContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name
                }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name
                }).FirstOrDefaultAsync();

            if (category == null)
                return NotFound();

            return Ok(category);
        }
    }
}