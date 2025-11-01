using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dishapi.Core.Dtos;
using Dishapi.Models;
using Dishapi.DAL;
using DalDish = Dishapi.DAL.Entities.Dish;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DishesController(AppDbContext context)
        {
            _context = context;
        }

        private Dish MapToDish(DalDish dalDish)
        {
            return new Dish
            {
                Id = dalDish.Id,
                Name = dalDish.Name ?? string.Empty,
                NameEn = dalDish.NameEn ?? string.Empty,
                Description = dalDish.Description ?? string.Empty,
                DescriptionEn = dalDish.DescriptionEn ?? string.Empty,
                Price = dalDish.Price,
                Category = dalDish.Category ?? string.Empty,
                CategoryEn = dalDish.CategoryEn ?? string.Empty,
                ImageUrl = dalDish.ImageUrl ?? string.Empty,
                IsAvailable = dalDish.IsAvailable,
                Vegetarian = dalDish.Vegetarian,
                Rating = dalDish.Rating ?? 0.0,
                CreatedAt = dalDish.CreatedAt
            };
        }

        private DalDish MapToDalDish(Dish dish)
        {
            return new DalDish
            {
                Id = dish.Id,
                Name = dish.Name ?? string.Empty,
                NameEn = dish.NameEn,
                Description = dish.Description,
                DescriptionEn = dish.DescriptionEn,
                Price = dish.Price,
                Category = dish.Category,
                CategoryEn = dish.CategoryEn,
                ImageUrl = dish.ImageUrl,
                IsAvailable = dish.IsAvailable,
                Vegetarian = dish.Vegetarian,
                Rating = dish.Rating,
                CreatedAt = dish.CreatedAt
            };
        }

        private DishDto MapToDto(Dish dish, string? language = "ru")
        {
            var isEnglish = (language ?? "ru").ToLowerInvariant() == "en";
            return new DishDto
            {
                Id = dish.Id,
                Name = isEnglish ? (dish.NameEn ?? dish.Name) : dish.Name,
                Description = isEnglish ? (dish.DescriptionEn ?? dish.Description) : dish.Description,
                Price = (double)dish.Price,
                Category = isEnglish ? (dish.CategoryEn ?? dish.Category) : dish.Category,
                ImageUrl = dish.ImageUrl ?? string.Empty,
                IsAvailable = dish.IsAvailable,
                Vegetarian = dish.Vegetarian,
                Rating = dish.Rating,
                CreatedAt = dish.CreatedAt
            };
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<DishDto>>> GetDishes(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? category = null,
            [FromQuery] string? search = null,
            [FromQuery] string? language = "ru")
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 5;
                if (pageSize > 100) pageSize = 100;

                var isEnglish = (language ?? "ru").ToLowerInvariant() == "en";

                var query = _context.Dishes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                {
                    var c = $"%{category.Trim()}%";
                    query = query.Where(d => isEnglish
                        ? EF.Functions.Like((d.CategoryEn ?? d.Category) ?? string.Empty, c)
                        : EF.Functions.Like(d.Category ?? string.Empty, c));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    query = query.Where(d => isEnglish
                        ? (((d.NameEn ?? d.Name) ?? string.Empty).Contains(s) || ((d.DescriptionEn ?? d.Description) ?? string.Empty).Contains(s))
                        : ((d.Name ?? string.Empty).Contains(s) || (d.Description ?? string.Empty).Contains(s)));
                }

                var items = await query
                    .OrderBy(d => d.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dishes = items.Select(d => MapToDish(d)).ToList();
                var dtos = dishes.Select(d => MapToDto(d, language)).ToList();

                return Ok(dtos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DishDto>> GetDish(int id, [FromQuery] string? language = "ru")
        {
            var dalDish = await _context.Dishes.FindAsync(id);
            if (dalDish == null) return NotFound(new { message = "Dish not found" });
            var dish = MapToDish(dalDish);
            return Ok(MapToDto(dish, language));
        }

        [HttpPost]
        public async Task<ActionResult<DishDto>> CreateDish([FromBody] Dish dish)
        {
            if (dish == null) return BadRequest(new { message = "Invalid dish data" });
            if (string.IsNullOrWhiteSpace(dish.Name) || dish.Price <= 0) return BadRequest(new { message = "Name and positive Price are required." });

            dish.CreatedAt = System.DateTime.UtcNow;
            var dalDish = MapToDalDish(dish);
            _context.Dishes.Add(dalDish);
            await _context.SaveChangesAsync();

            dish.Id = dalDish.Id;
            var dto = MapToDto(dish);
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DishDto>> UpdateDish(int id, [FromBody] Dish dish)
        {
            if (dish == null) return BadRequest(new { message = "Invalid dish data" });

            var existingDish = await _context.Dishes.FindAsync(id);
            if (existingDish == null) return NotFound(new { message = "Dish not found" });
            if (string.IsNullOrWhiteSpace(dish.Name) || dish.Price <= 0) return BadRequest(new { message = "Name and positive Price are required." });

            existingDish.Name = dish.Name;
            existingDish.NameEn = dish.NameEn;
            existingDish.Description = dish.Description;
            existingDish.DescriptionEn = dish.DescriptionEn;
            existingDish.Price = dish.Price;
            existingDish.Category = dish.Category;
            existingDish.CategoryEn = dish.CategoryEn;
            existingDish.ImageUrl = dish.ImageUrl;
            existingDish.IsAvailable = dish.IsAvailable;
            existingDish.Vegetarian = dish.Vegetarian;
            existingDish.Rating = dish.Rating;
            existingDish.CreatedAt = dish.CreatedAt;

            await _context.SaveChangesAsync();

            var updatedDish = MapToDish(existingDish);
            var dto = MapToDto(updatedDish);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound(new { message = "Dish not found" });
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<string>>> GetCategories([FromQuery] string? language = "ru")
        {
            var isEnglish = (language ?? "ru").ToLowerInvariant() == "en";
            var categories = await _context.Dishes
                .Select(d => isEnglish ? (d.CategoryEn ?? d.Category) : d.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
            return Ok(categories);
        }
    }
}