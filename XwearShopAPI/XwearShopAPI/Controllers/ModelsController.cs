using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly DbExwearContext _context;

        public ModelsController(DbExwearContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var models = await _context.Models
                .Select(m => new
                {
                    id = m.Id,
                    name = m.Name
                }).ToListAsync();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var model = await _context.Models.Where(m => m.Id == id)
                .Select(m => new
                {
                    id = m.Id,
                    name = m.Name
                }).FirstOrDefaultAsync();

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}