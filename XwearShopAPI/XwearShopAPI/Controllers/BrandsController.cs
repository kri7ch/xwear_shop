using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly DbExwearContext _context;

        public BrandsController(DbExwearContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var brands = await _context.Brands.Select(b => new
                {
                    id = b.Id,
                    name = b.Name
                }).ToListAsync();

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var brand = await _context.Brands.Where(b => b.Id == id).Select(b => new
                {
                    id = b.Id,
                    name = b.Name
                }).FirstOrDefaultAsync();

            if (brand == null)
                return NotFound();

            return Ok(brand);
        }
    }
}