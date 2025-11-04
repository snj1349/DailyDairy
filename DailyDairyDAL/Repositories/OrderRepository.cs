using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class OrderRepository : IOrderRepository
{
	private readonly DailyDairyDbContext _context;

	public OrderRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Order>> GetAllAsync()
	{
		return await _context.Orders.AsNoTracking().ToListAsync();
	}

	public async Task<Order?> GetByIdAsync(int id)
	{
		return await _context.Orders
			.Include(o => o.OrderItems)
			.Include(o => o.Payments)
			.AsNoTracking()
			.FirstOrDefaultAsync(o => o.OrderId == id);
	}

	public async Task<Order> AddAsync(Order entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Orders.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Order entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Orders.AnyAsync(o => o.OrderId == entity.OrderId);
		if (!exists) return false;
		_context.Orders.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
		if (existing == null) return false;
		_context.Orders.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
	{
		return await _context.Orders.AsNoTracking().Where(o => o.UserId == userId).ToListAsync();
	}

	public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
	{
		if (string.IsNullOrWhiteSpace(status)) return false;
		var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
		if (order == null) return false;
		order.Status = status;
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
	{
		return await _context.Orders.AsNoTracking().Where(o => o.Status == "Pending").ToListAsync();
	}

	public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
	{
		return await _context.Orders.AsNoTracking()
			.Where(o => o.OrderDate >= start && o.OrderDate <= end)
			.ToListAsync();
	}
}


