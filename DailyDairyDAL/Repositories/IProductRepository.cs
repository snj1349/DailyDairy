using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetAllAsync();
	Task<Product?> GetByIdAsync(int id);
	Task<Product> AddAsync(Product entity);
	Task<bool> UpdateAsync(Product entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
	Task<IEnumerable<Product>> SearchAsync(string keyword);
	Task<bool> UpdateStockAsync(int productId, int newQuantity);
	Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
	Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count);
}


