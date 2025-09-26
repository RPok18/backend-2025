using Microsoft.AspNetCore.Mvc;
using Dishapi.Models;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private static List<Dish> _dishes = new()
        {
            new Dish {
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
                Rating = 6.50426849970817
            },
            new Dish {
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
                Rating = 5.847521865882921
            },
            new Dish {
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
                Rating = 3.2222222222222223
            },
            new Dish {
                Id = 4,
                Name = "Вок болоньезе",
                NameEn = "Wok Bolognese",
                Description = "Пшеничная лапша обжаренная на воке с фаршем (говядина/свинина) и овощами (шампиньоны, перец сладкий, лук красный)",
                DescriptionEn = "Wheat noodles stir-fried in a wok with minced meat (beef/pork) and vegetables (mushrooms, bell pepper, red onion)",
                Price = 280.00m,
                Category = "Вок",
                CategoryEn = "Wok",
                ImageUrl = "https://mistertako.ru/uploads/products/a41bd9fd-54ed-11ed-8575-005059dbef0.jpg",
                IsAvailable = false,
                Vegetarian = false,
                Rating = 9.0
            },
            new Dish {
                Id = 5,
                Name = "Том Ям",
                NameEn = "Tom Yum",
                Description = "Лапша пшеничная, куриное филе, шампиньоны, лук красный, запрака Том Ям (паста Том Ям, паста Том Кха, сахар, соевый соус), сливки, соевый соус, помидор, перец чили",
                DescriptionEn = "Wheat noodles, chicken fillet, mushrooms, red onion, Tom Yum paste (Tom Yum paste, Tom Kha paste, sugar, soy sauce), cream, soy sauce, tomato, chili pepper",
                Price = 280.00m,
                Category = "Вок",
                CategoryEn = "Wok",
                ImageUrl = "https://mistertako.ru/uploads/products/9975bc37a-b453-4fb2-273e-c8dbc899a338",
                IsAvailable = false,
                Vegetarian = false,
                Rating = 9.0
            },
            new Dish {
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
                Rating = 4.5
            },
            new Dish {
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
                Rating = 4.2
            },
            new Dish {
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
                Rating = 4.8
            }
        };

        private DishDto MapToDto(Dish dish, string language = "ru")
        {
            bool isEnglish = language.ToLower() == "en";

            return new DishDto
            {
                Id = dish.Id,
                Name = isEnglish ? dish.NameEn : dish.Name,
                Description = isEnglish ? dish.DescriptionEn : dish.Description,
                Price = dish.Price,
                Category = isEnglish ? dish.CategoryEn : dish.Category,
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
            [FromQuery] string language = "ru")
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 5;
                if (pageSize > 100) pageSize = 100;

                bool isEnglish = language.ToLower() == "en";
                var filteredDishes = _dishes.AsQueryable();

                // Filter by category
                if (!string.IsNullOrEmpty(category))
                {
                    filteredDishes = filteredDishes.Where(d =>
                        (isEnglish ? d.CategoryEn : d.Category).Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                // Filter by search term
                if (!string.IsNullOrEmpty(search))
                {
                    filteredDishes = filteredDishes.Where(d =>
                        (isEnglish ? d.NameEn : d.Name).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (isEnglish ? d.DescriptionEn : d.Description).Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                var totalItems = filteredDishes.Count();
                var dishes = filteredDishes
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(d => MapToDto(d, language))
                    .ToList();

                var pagination = new Pagination(page, pageSize, totalItems);
                var response = new DishResponse(dishes, pagination);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DishResponse($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DishDto> GetDish(int id, [FromQuery] string language = "ru")
        {
            var dish = _dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound(new { message = "Dish not found" });
            }
            return Ok(MapToDto(dish, language));
        }

        [HttpPost]
        public ActionResult<Dish> CreateDish([FromBody] Dish dish)
        {
            if (dish == null)
            {
                return BadRequest(new { message = "Invalid dish data" });
            }

            dish.Id = _dishes.Max(d => d.Id) + 1;
            dish.CreatedAt = DateTime.Now;
            _dishes.Add(dish);

            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, dish);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDish(int id)
        {
            var dish = _dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound(new { message = "Dish not found" });
            }

            _dishes.Remove(dish);
            return NoContent();
        }

        [HttpGet("categories")]
        public ActionResult<List<string>> GetCategories([FromQuery] string language = "ru")
        {
            bool isEnglish = language.ToLower() == "en";
            var categories = _dishes
                .Select(d => isEnglish ? d.CategoryEn : d.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            return Ok(categories);
        }
    }
}