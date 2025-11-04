using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class CartRepository : ICartRepository
{
	private readonly DailyDairyDbContext _context;

	public CartRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Cart>> GetAllAsync()
	{
		return await _context.Carts.AsNoTracking().ToListAsync();
	}

	public async Task<Cart?> GetByIdAsync(int id)
	{
		return await _context.Carts.AsNoTracking().FirstOrDefaultAsync(c => c.CartId == id);
	}

	public async Task<Cart> AddAsync(Cart entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Carts.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Cart entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Carts.AnyAsync(c => c.CartId == entity.CartId);
		if (!exists) return false;
		_context.Carts.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == id);
		if (existing == null) return false;
		_context.Carts.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Cart>> GetCartItemsByUserAsync(int userId)
	{
		return await _context.Carts
			.Include(c => c.Product)
			.AsNoTracking()
			.Where(c => c.UserId == userId)
			.ToListAsync();
	}

	public async Task<bool> ClearCartAsync(int userId)
	{
		var items = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
		if (items.Count == 0) return true;
		_context.Carts.RemoveRange(items);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<decimal> GetCartTotalAsync(int userId)
	{
		var total = await _context.Carts
			.Where(c => c.UserId == userId)
			.Join(_context.Products,
				c => c.ProductId,
				p => p.ProductId,
				(c, p) => new { c.Quantity, p.Price })
			.Select(x => (decimal)(x.Quantity) * x.Price)
			.SumAsync();
		return total;
	}
}


