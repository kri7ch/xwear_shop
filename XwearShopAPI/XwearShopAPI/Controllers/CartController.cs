using System.Security.Claims;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly DbExwearContext _db;

        public CartController(DbExwearContext db)
        {
            _db = db;
        }

        private int? GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return null;
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var rows = await _db.Carts.Where(c => c.UserId == userId).Include(c => c.Product).ThenInclude(p => p.Brand).Include(c => c.Product).ThenInclude(p => p.Images).Include(c => c.Size).ToListAsync();

            var responseItems = new List<object>();
            float totalPrice = 0;
            int totalItems = 0;

            foreach (var row in rows)
            {
                var p = row.Product;
                if (p == null) continue;

                var price = row.Size?.Price ?? 0;
                var imageUrl = BuildImageUrl(p.Images.FirstOrDefault(i => i.IsMain == 1) ?? p.Images.FirstOrDefault());

                var lineTotal = price * row.Count;
                totalPrice += lineTotal;
                totalItems += row.Count;

                responseItems.Add(new
                {
                    productId = p.Id,
                    name = p.Name,
                    brand = p.Brand.Name,
                    size = row.Size?.Size1,
                    price,
                    quantity = row.Count,
                    imageUrl,
                    lineTotal
                });
            }

            return Ok(new { items = responseItems, totalItems, totalPrice });
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Cart incoming, [FromQuery] string size, [FromQuery] int quantity = 1)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var productId = incoming.ProductId;
            if (productId <= 0) return BadRequest(new { message = "productId_required" });

            quantity = Math.Max(1, quantity);

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound(new { message = "product_not_found" });

            if (!float.TryParse(size, NumberStyles.Float, CultureInfo.InvariantCulture, out var sizeValue))
                return BadRequest(new { message = "size_invalid" });
            var sizeRow = await _db.Sizes.FirstOrDefaultAsync(s => s.ProductId == productId && Math.Abs(s.Size1 - sizeValue) < 0.0001f);
            if (sizeRow == null) return NotFound(new { message = "size_not_found" });

            var existing = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.SizeId == sizeRow.Id);
            if (existing == null)
            {
                _db.Carts.Add(new Cart { UserId = userId.Value, ProductId = productId, SizeId = sizeRow.Id, Count = quantity });
            }
            else
            {
                existing.Count += quantity;
            }
            await _db.SaveChangesAsync();
            return await GetCart();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Cart incoming, [FromQuery] string size, [FromQuery] int quantity)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            if (quantity < 0) return BadRequest(new { message = "quantity_invalid" });

            var productId = incoming.ProductId;
            if (productId <= 0) return BadRequest(new { message = "productId_required" });

            if (!float.TryParse(size, NumberStyles.Float, CultureInfo.InvariantCulture, out var sizeValue))
                return BadRequest(new { message = "size_invalid" });
            var sizeRow = await _db.Sizes.FirstOrDefaultAsync(s => s.ProductId == productId && Math.Abs(s.Size1 - sizeValue) < 0.0001f);
            if (sizeRow == null) return NotFound(new { message = "size_not_found" });

            var existing = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.SizeId == sizeRow.Id);
            if (existing == null)
            {
                return NotFound(new { message = "item_not_found" });
            }

            if (quantity <= 0)
            {
                _db.Carts.Remove(existing);
            }
            else
            {
                existing.Count = quantity;
            }
            await _db.SaveChangesAsync();
            return await GetCart();
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> Remove([FromBody] Cart incoming, [FromQuery] string size)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var productId = incoming.ProductId;
            if (productId <= 0) return BadRequest(new { message = "productId_required" });

            if (!float.TryParse(size, NumberStyles.Float, CultureInfo.InvariantCulture, out var sizeValue))
                return BadRequest(new { message = "size_invalid" });
            var sizeRow = await _db.Sizes.FirstOrDefaultAsync(s => s.ProductId == productId && Math.Abs(s.Size1 - sizeValue) < 0.0001f);
            if (sizeRow == null) return NotFound(new { message = "size_not_found" });

            var existing = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.SizeId == sizeRow.Id);
            if (existing == null)
                return NotFound(new { message = "item_not_found" });

            _db.Carts.Remove(existing);
            await _db.SaveChangesAsync();
            return await GetCart();
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