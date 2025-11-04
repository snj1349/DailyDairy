using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface ICategoryRepository
{
	Task<IEnumerable<Category>> GetAllAsync();
	Task<Category?> GetByIdAsync(int id);
	Task<Category> AddAsync(Category entity);
	Task<bool> UpdateAsync(Category entity);
	Task<bool> DeleteAsync(int id);

	Task<Category?> GetCategoryWithProductsAsync(int categoryId);
}


