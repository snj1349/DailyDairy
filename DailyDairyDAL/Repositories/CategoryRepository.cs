using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class CategoryRepository : ICategoryRepository
{
	private readonly DailyDairyDbContext _context;

	public CategoryRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Category>> GetAllAsync()
	{
		return await _context.Categories.AsNoTracking().ToListAsync();
	}

	public async Task<Category?> GetByIdAsync(int id)
	{
		return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
	}

	public async Task<Category> AddAsync(Category entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Categories.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Category entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Categories.AnyAsync(c => c.CategoryId == entity.CategoryId);
		if (!exists) return false;
		_context.Categories.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
		if (existing == null) return false;
		_context.Categories.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
	{
		return await _context.Categories
			.Include(c => c.Products)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
	}
}


