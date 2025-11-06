using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly DbExwearContext _db;

        public ProductsController(DbExwearContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Model).Include(p => p.Images).Include(p => p.Sizes).ToListAsync();

            var result = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                brand = p.Brand.Name,
                category = p.Category.Name,
                model = p.Model.Name,
                mainImageUrl = BuildImageUrl(p.Images.FirstOrDefault(i => i.IsMain == 1) ?? p.Images.FirstOrDefault()),
                sizes = p.Sizes.Select(s => new { size = s.Size1, price = s.Price }).OrderBy(s => s.size).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Model).Include(p => p.Images).Include(p => p.Sizes).FirstOrDefaultAsync(p => p.Id == id);
            if (p == null)
                return NotFound();

            var result = new
            {
                id = p.Id,
                name = p.Name,
                brand = p.Brand.Name,
                category = p.Category.Name,
                model = p.Model.Name,
                mainImageUrl = BuildImageUrl(p.Images.FirstOrDefault(i => i.IsMain == 1) ?? p.Images.FirstOrDefault()),
                images = p.Images.Select(i => BuildImageUrl(i)).Where(u => !string.IsNullOrEmpty(u)).ToList(),
                sizes = p.Sizes.Select(s => new { size = s.Size1, price = s.Price }).OrderBy(s => s.size).ToList()
            };

            return Ok(result);
        }

        private string BuildImageUrl(Image? image)
        {
            string relative = image?.Path ?? "\\Images\\Products\\plug.svg";
            relative = relative.Replace("\\", "/");

            if (!relative.StartsWith("/"))
                relative = "/" + relative;

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return $"{baseUrl}/assets{relative}";
        }
    }
}