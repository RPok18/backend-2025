using Dishapi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.DAL
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            
            if (await context.Dishes.AnyAsync())
                return;

            var dishes = new List<Dish>
            {
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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
                new Dish
                {
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

            context.Dishes.AddRange(dishes);
            await context.SaveChangesAsync();
        }
    }
}