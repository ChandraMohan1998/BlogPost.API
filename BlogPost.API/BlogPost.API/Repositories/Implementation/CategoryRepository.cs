using BlogPost.API.Data;
using BlogPost.API.Model.Domain;
using BlogPost.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BlogPost.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext _dBContext;
        public CategoryRepository(ApplicationDbContext dBContext)
        {
            this._dBContext = dBContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dBContext.Categories.AddAsync(category);
            await _dBContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dBContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _dBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _dBContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCategory != null) 
            {
                _dBContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await _dBContext.SaveChangesAsync();
                return category;
            }

            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _dBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory != null)
            {
                _dBContext.Categories.Remove(existingCategory);
                await _dBContext.SaveChangesAsync();
                return existingCategory;
            }

            return null;
        }
    }
}
