using Microsoft.AspNetCore.Mvc;
using Dishapi.Models;

namespace Dishapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        // Use fully qualified Dish class to avoid namespace conflicts
        private static readonly List<Dishapi.Models.Dish> Dishes = new()
        {
            new Dishapi.Models.Dish
            {
                Id = Guid.NewGuid(),
                Name = "Pizza",
                Price = 9.99m,
                Description = "Cheesy pizza with tomato sauce"
            },
            new Dishapi.Models.Dish
            {
                Id = Guid.NewGuid(),
                Name = "Burger",
                Price = 6.49m,
                Description = "Beef burger with lettuce"
            },
            new Dishapi.Models.Dish
            {
                Id = Guid.NewGuid(),
                Name = "Pasta",
                Price = 7.99m,
                Description = "Pasta with creamy sauce"
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Dishapi.Models.Dish>> GetAll() => Ok(Dishes);

        [HttpGet("{id:guid}")]
        public ActionResult<Dishapi.Models.Dish> GetById(Guid id)
        {
            var dish = Dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null) return NotFound();
            return Ok(dish);
        }

        [HttpPost]
        public ActionResult<Dishapi.Models.Dish> Create([FromBody] Dishapi.Models.Dish newDish)
        {
            newDish.Id = Guid.NewGuid();
            Dishes.Add(newDish);
            return CreatedAtAction(nameof(GetById), new { id = newDish.Id }, newDish);
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var existing = Dishes.FirstOrDefault(d => d.Id == id);
            if (existing == null) return NotFound();

            Dishes.Remove(existing);
            return NoContent();
        }
    }
}
