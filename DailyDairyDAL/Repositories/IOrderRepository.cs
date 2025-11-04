using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IOrderRepository
{
	Task<IEnumerable<Order>> GetAllAsync();
	Task<Order?> GetByIdAsync(int id);
	Task<Order> AddAsync(Order entity);
	Task<bool> UpdateAsync(Order entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
	Task<bool> UpdateOrderStatusAsync(int orderId, string status);
	Task<IEnumerable<Order>> GetPendingOrdersAsync();
	Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end);
}


