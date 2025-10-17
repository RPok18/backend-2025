using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;
using Dishapi.Core.Models;
using Dishapi.Models;


using DalDish = Dishapi.DAL.Entities.Dish;
using WebDish = Dishapi.Models.Dish;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, WebDish> _dishes;
        private static int _nextId;

        static DishesController()
        {
            var initial = new List<WebDish>
            {
                new WebDish {
                    Id = 1,
                    Name = "4 сыра",
                    NameEn = "4 Cheese Pizza",
                    Description = "4 сыра: «Моцарелла», «Гауда», «Фета», «Дор-блю», сливочно-сырный соус, пряные травы",
                    DescriptionEn = "4 cheeses: Mozzarella, Gouda, Feta, Blue cheese, creamy cheese sauce, herbs",
                    Price = 360.00m,
                    Category = "Пицца",
                    CategoryEn = "Pizza",
                    ImageUrl = "https://mistertako.ru/uploads/products/778887e-8327-11ec-8575-005059dbef0..",
                    IsAvailable = true,
                    Vegetarian = true,
                    Rating = 6.5,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 2,
                    Name = "Party BBQ",
                    NameEn = "Party BBQ Pizza",
                    Description = "Бекон, соленый огурчик, брусника, сыр моцарелла, соус барбекю",
                    DescriptionEn = "Bacon, pickled cucumber, cranberries, mozzarella cheese, BBQ sauce",
                    Price = 480.00m,
                    Category = "Пицца",
                    CategoryEn = "Pizza",
                    ImageUrl = "https://mistertako.ru/uploads/products/659ab866-85ec-11ea-a9ab-005059dbef0..",
                    IsAvailable = false,
                    Vegetarian = false,
                    Rating = 5.8,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 3,
                    Name = "Вок а-ля Диабло",
                    NameEn = "Wok a-la Diablo",
                    Description = "Пшеничная лапша обжаренная на воке с колбасками чоризо",
                    DescriptionEn = "Wheat noodles stir-fried in a wok with chorizo sausages",
                    Price = 340.00m,
                    Category = "Вок",
                    CategoryEn = "Wok",
                    ImageUrl = "https://mistertako.ru/uploads/products/663ab868-85ec-11ea-a9ab-86b1f8341741.jpg",
                    IsAvailable = false,
                    Vegetarian = false,
                    Rating = 3.2,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 4,
                    Name = "Вок болоньезе",
                    NameEn = "Wok Bolognese",
                    Description = "Пшеничная лапша с фаршем и овощами",
                    DescriptionEn = "Wheat noodles with minced meat and vegetables",
                    Price = 280.00m,
                    Category = "Вок",
                    CategoryEn = "Wok",
                    ImageUrl = "https://mistertako.ru/uploads/products/a41bd9fd-54ed-11ed-8575-005059dbef0.jpg",
                    IsAvailable = false,
                    Vegetarian = false,
                    Rating = 9.0,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 5,
                    Name = "Том Ям",
                    NameEn = "Tom Yum",
                    Description = "Лапша пшеничная, куриное филе, шампиньоны, соус Том Ям",
                    DescriptionEn = "Wheat noodles, chicken fillet, mushrooms, Tom Yum sauce",
                    Price = 280.00m,
                    Category = "Вок",
                    CategoryEn = "Wok",
                    ImageUrl = "https://mistertako.ru/uploads/products/9975bc37a-b453-4fb2-273e-c8dbc899a338",
                    IsAvailable = false,
                    Vegetarian = false,
                    Rating = 9.0,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 6,
                    Name = "Цезарь с курицей",
                    NameEn = "Caesar Salad with Chicken",
                    Description = "Салат Айсберг, куриная грудка, помидоры черри, сыр пармезан, соус цезарь",
                    DescriptionEn = "Iceberg lettuce, chicken breast, cherry tomatoes, parmesan cheese, caesar dressing",
                    Price = 220.00m,
                    Category = "Салаты",
                    CategoryEn = "Salads",
                    ImageUrl = "https://mistertako.ru/uploads/products/caesar-chicken.jpg",
                    IsAvailable = true,
                    Vegetarian = false,
                    Rating = 4.5,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 7,
                    Name = "Греческий салат",
                    NameEn = "Greek Salad",
                    Description = "Помидоры, огурцы, красный лук, маслины, сыр фета, оливковое масло",
                    DescriptionEn = "Tomatoes, cucumbers, red onion, olives, feta cheese, olive oil",
                    Price = 190.00m,
                    Category = "Салаты",
                    CategoryEn = "Salads",
                    ImageUrl = "https://mistertako.ru/uploads/products/greek-salad.jpg",
                    IsAvailable = true,
                    Vegetarian = true,
                    Rating = 4.2,
                    CreatedAt = DateTime.UtcNow
                },
                new WebDish {
                    Id = 8,
                    Name = "Тирамису",
                    NameEn = "Tiramisu",
                    Description = "Классический итальянский десерт с кофе и маскарпоне",
                    DescriptionEn = "Classic Italian dessert with coffee and mascarpone",
                    Price = 150.00m,
                    Category = "Десерты",
                    CategoryEn = "Desserts",
                    ImageUrl = "https://mistertako.ru/uploads/products/tiramisu.jpg",
                    IsAvailable = true,
                    Vegetarian = true,
                    Rating = 4.8,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _dishes = new ConcurrentDictionary<int, WebDish>(initial.ToDictionary(d => d.Id, d => d));
            _nextId = initial.Any() ? initial.Max(d => d.Id) + 1 : 1;
        }

        private DishDto MapToDto(WebDish dish, string? language = "ru")
        {
            var isEnglish = (language ?? "ru").ToLowerInvariant() == "en";
            return new DishDto
            {
                Id = dish.Id,
                Name = isEnglish ? (dish.NameEn ?? dish.Name) : dish.Name,
                Description = isEnglish ? (dish.DescriptionEn ?? dish.Description) : dish.Description,
                Price = dish.Price,
                Category = isEnglish ? (dish.CategoryEn ?? dish.Category) : dish.Category,
                ImageUrl = dish.ImageUrl,
                IsAvailable = dish.IsAvailable,
                Vegetarian = dish.Vegetarian,
                Rating = dish.Rating,
                CreatedAt = dish.CreatedAt
            };
        }

        [HttpGet]
        public ActionResult<DishResponse> GetDishes(
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

                var all = _dishes.Values.AsQueryable();

                if (!string.IsNullOrWhiteSpace(category))
                {
                    var c = category.Trim();
                    all = all.Where(d => string.Equals(isEnglish ? d.CategoryEn : d.Category, c, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    all = all.Where(d =>
                        (isEnglish ? (d.NameEn ?? string.Empty) : (d.Name ?? string.Empty)).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (isEnglish ? (d.DescriptionEn ?? string.Empty) : (d.Description ?? string.Empty)).IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }

                var totalItems = all.Count();
                var items = all
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(d => MapToDto(d, language ?? "ru"))
                    .ToList();

                var pagination = new Pagination(page, pageSize, totalItems);
                var response = new DishResponse(items, pagination);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DishResponse($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DishDto> GetDish(int id, [FromQuery] string? language = "ru")
        {
            if (_dishes.TryGetValue(id, out var dish))
            {
                return Ok(MapToDto(dish, language));
            }
            return NotFound(new { message = "Dish not found" });
        }

        [HttpPost]
        public ActionResult<DishDto> CreateDish([FromBody] WebDish dish)
        {
            if (dish == null)
                return BadRequest(new { message = "Invalid dish data" });

            if (string.IsNullOrWhiteSpace(dish.Name) || dish.Price <= 0)
                return BadRequest(new { message = "Name and positive Price are required." });

            var id = Interlocked.Increment(ref _nextId);
            dish.Id = id;
            dish.CreatedAt = DateTime.UtcNow;

            if (!_dishes.TryAdd(dish.Id, dish))
                return StatusCode(500, new { message = "Could not add dish due to concurrency issue." });

            var dto = MapToDto(dish);
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dto);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDish(int id)
        {
            if (!_dishes.TryRemove(id, out _))
                return NotFound(new { message = "Dish not found" });

            return NoContent();
        }

        [HttpGet("categories")]
        public ActionResult<List<string>> GetCategories([FromQuery] string? language = "ru")
        {
            var isEnglish = (language ?? "ru").ToLowerInvariant() == "en";
            var categories = _dishes.Values
                .Select(d => isEnglish ? (d.CategoryEn ?? d.Category) : d.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c)
                .ToList();

            return Ok(categories);
        }
    }
}
