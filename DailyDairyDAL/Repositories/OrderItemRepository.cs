using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
	private readonly DailyDairyDbContext _context;

	public OrderItemRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<OrderItem>> GetAllAsync()
	{
		return await _context.OrderItems.AsNoTracking().ToListAsync();
	}

	public async Task<OrderItem?> GetByIdAsync(int id)
	{
		return await _context.OrderItems.AsNoTracking().FirstOrDefaultAsync(oi => oi.OrderItemId == id);
	}

	public async Task<OrderItem> AddAsync(OrderItem entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.OrderItems.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(OrderItem entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.OrderItems.AnyAsync(oi => oi.OrderItemId == entity.OrderItemId);
		if (!exists) return false;
		_context.OrderItems.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.OrderItems.FirstOrDefaultAsync(oi => oi.OrderItemId == id);
		if (existing == null) return false;
		_context.OrderItems.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
	{
		return await _context.OrderItems
			.Include(oi => oi.Product)
			.AsNoTracking()
			.Where(oi => oi.OrderId == orderId)
			.ToListAsync();
	}
}


