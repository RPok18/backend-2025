using AutoMapper;
using Dishapi.Core.Dtos;
using Dishapi.DAL.Entities;
using Dishapi.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dishapi.BLL.Services
{
    public class DishService : IDishService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DishService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DishDto>> GetAllDishesAsync()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }

        public async Task<DishDto?> GetDishByIdAsync(int id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            return dish == null ? null : _mapper.Map<DishDto>(dish);
        }

        public async Task<DishDto> CreateDishAsync(DishCreateDto createDishDto)
        {
            var dish = _mapper.Map<Dish>(createDishDto);
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            var createdDish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dish.Id);
            return _mapper.Map<DishDto>(createdDish);
        }

        public async Task<DishDto?> UpdateDishAsync(int id, DishUpdateDto updateDishDto)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return null;

            _mapper.Map(updateDishDto, dish);
            await _context.SaveChangesAsync();

            var updatedDish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            return _mapper.Map<DishDto>(updatedDish);
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return false;

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DishDto>> GetDishesByProfileIdAsync(int profileId)
        {
            var dishes = await _context.Dishes.ToListAsync();
            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }
    }
}