using Microsoft.AspNetCore.Mvc;
using DishApi.Models;

namespace DishApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DishesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDishes()
        {
            var response = new DishResponse
            {
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "4 сыра",
                        Description = "4 сыра: «Моцарелла», «Гауда», «Фета», «Дор-блю», сливочно-сырный соус, пряные травы",
                        Price = 360.00,
                        Image = "https://mistertako.ru/uploads/products/77888c7e-8327-11ec-8575-0050569dbef0.jpg",
                        Vegetarian = true,
                        Rating = 6.504268499707817,
                        Category = "Pizza",
                        Id = "e4698ac7-7d9e-456e-2741-08dbc899a338"
                    },
                    new Dish
                    {
                        Name = "Party BBQ",
                        Description = "Бекон, соленый огурчик, брусника, сыр «Моцарелла», сыр «Гауда», соус BBQ",
                        Price = 480.00,
                        Image = "https://mistertako.ru/uploads/products/839d0250-8327-11ec-8575-0050569dbef0.jpg",
                        Vegetarian = false,
                        Rating = 5.844752186588921,
                        Category = "Pizza",
                        Id = "a0ea8caf-e461-4877-2742-08dbc899a338"
                    },
                    new Dish
                    {
                        Name = "Wok а-ля Диабло",
                        Description = "Пшеничная лапша обжаренная на воке с колбасками пепперони, маслинами, сладким перцем и перцем халапеньо в томатном соусе с добавлением петрушки. БЖУ на 100 г. Белки, г — 8,18 Жиры, г — 16,33 Углеводы, г — 20,62",
                        Price = 330.00,
                        Image = "https://mistertako.ru/uploads/products/663ab868-85ec-11ea-a9ab-86b1f8341741.jpg",
                        Vegetarian = false,
                        Rating = 3.2222222222222223,
                        Category = "Wok",
                        Id = "3b690ced-a766-451c-273c-08dbc899a338"
                    },
                    new Dish
                    {
                        Name = "Wok болоньезе",
                        Description = "Пшеничная лапша обжаренная на воке с фаршем (Говядина/свинина) и овощами (шампиньоны, перец сладкий, лук красный) в томатном соусе с добавлением чесночно-имбирной заправки и петрушки. БЖУ на 100 г. Белки, г — 8,07 Жиры, г — 15,38 Углеводы, г — 23,22",
                        Price = 290.00,
                        Image = "https://mistertako.ru/uploads/products/663ab866-85ec-11ea-a9ab-86b1f8341741.jpg",
                        Vegetarian = false,
                        Rating = 9.0,
                        Category = "Wok",
                        Id = "c63f070c-2f34-4760-273d-08dbc899a338"
                    },
                    new Dish
                    {
                        Name = "Wok том ям с курицей",
                        Description = "Лапша пшеничная, куриное филе, шампиньоны, лук красный, заправка Том Ям (паста Том Ям, паста Том Кха, соевый соус), сливки, соевый соус, помидор, перец чили. БЖУ на 100 г. Белки, г — 7,05 Жиры, г — 12,92 Углеводы, г — 18,65",
                        Price = 280.00,
                        Image = "https://mistertako.ru/uploads/products/a41bd9fd-54ed-11ed-8575-0050569dbef0.jpg",
                        Vegetarian = false,
                        Rating = 9.0,
                        Category = "Wok",
                        Id = "975bc37a-b453-4fb2-273e-08dbc899a338"
                    }
                },
                Pagination = new Pagination
                {
                    Size = 5,
                    Count = 4,
                    Current = 1
                }
            };

            return Ok(response);
        }
    }
}
