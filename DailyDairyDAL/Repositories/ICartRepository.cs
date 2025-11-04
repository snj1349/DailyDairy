using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface ICartRepository
{
	Task<IEnumerable<Cart>> GetAllAsync();
	Task<Cart?> GetByIdAsync(int id);
	Task<Cart> AddAsync(Cart entity);
	Task<bool> UpdateAsync(Cart entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<Cart>> GetCartItemsByUserAsync(int userId);
	Task<bool> ClearCartAsync(int userId);
	Task<decimal> GetCartTotalAsync(int userId);
}


