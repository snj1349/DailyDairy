using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IOrderItemRepository
{
	Task<IEnumerable<OrderItem>> GetAllAsync();
	Task<OrderItem?> GetByIdAsync(int id);
	Task<OrderItem> AddAsync(OrderItem entity);
	Task<bool> UpdateAsync(OrderItem entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
}


