using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly DbExwearContext _db;

        public OrdersController(DbExwearContext db)
        {
            _db = db;
        }

        private int? GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return null;
            if (int.TryParse(userIdStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
                return id;
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized(new { message = "unauthorized" });

            var list = await _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var response = list.Select(o => new
            {
                id = o.Id,
                date = o.CreatedAt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                createdAt = o.CreatedAt,
                status = "создан",
                totalAmount = o.TotalAmount,
                isPaid = o.IsPaid
            }).ToList();

            return Ok(response);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetMyOrdersHistory()
        {
            return await GetMyOrders();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromQuery] string? payment, [FromBody] Order input)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized(new { message = "unauthorized" });

            var payMethod = (payment ?? "cash").Trim().ToLowerInvariant();
            if (payMethod != "cash" && payMethod != "card")
            {
                return BadRequest(new { message = "payment_invalid" });
            }

            if (string.IsNullOrWhiteSpace(input.Country) ||
                string.IsNullOrWhiteSpace(input.Street) ||
                string.IsNullOrWhiteSpace(input.City) ||
                string.IsNullOrWhiteSpace(input.NumberHome) ||
                string.IsNullOrWhiteSpace(input.District) ||
                string.IsNullOrWhiteSpace(input.Index))
            {
                return BadRequest(new { message = "address_invalid" });
            }

            var cartItems = await _db.Carts
                .Include(c => c.Size)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return BadRequest(new { message = "cart_empty" });
            }

            var total = cartItems.Sum(c => (c.Size?.Price ?? 0f) * c.Count);

            var order = new Order
            {
                UserId = userId.Value,
                Country = input.Country!.Trim(),
                Street = input.Street!.Trim(),
                City = input.City!.Trim(),
                NumberHome = input.NumberHome!.Trim(),
                District = input.District!.Trim(),
                Index = input.Index!.Trim(),
                Status = "создан",
                TotalAmount = total,
                IsPaid = (sbyte)(payMethod == "card" ? 1 : 0),
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    _db.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId
                    });
                }
            }

            _db.Carts.RemoveRange(cartItems);

            await _db.SaveChangesAsync();

            return Ok(new { id = order.Id, message = "order_created" });
        }
    }
}