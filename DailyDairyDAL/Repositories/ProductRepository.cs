using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class ProductRepository : IProductRepository
{
	private readonly DailyDairyDbContext _context;

	public ProductRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Product>> GetAllAsync()
	{
		return await _context.Products.AsNoTracking().ToListAsync();
	}

	public async Task<Product?> GetByIdAsync(int id)
	{
		return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
	}

	public async Task<Product> AddAsync(Product entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Products.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Product entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Products.AnyAsync(p => p.ProductId == entity.ProductId);
		if (!exists) return false;
		_context.Products.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
		if (existing == null) return false;
		_context.Products.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
	{
		return await _context.Products.AsNoTracking().Where(p => p.CategoryId == categoryId).ToListAsync();
	}

	public async Task<IEnumerable<Product>> SearchAsync(string keyword)
	{
		if (string.IsNullOrWhiteSpace(keyword)) return Array.Empty<Product>();
		var like = $"%{keyword.Trim()}%";
		return await _context.Products.AsNoTracking()
			.Where(p => EF.Functions.Like(p.Name, like) || (p.Description != null && EF.Functions.Like(p.Description, like)))
			.ToListAsync();
	}

	public async Task<bool> UpdateStockAsync(int productId, int newQuantity)
	{
		if (newQuantity < 0) return false;
		var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
		if (product == null) return false;
		product.StockQuantity = newQuantity;
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
	{
		return await _context.Products.AsNoTracking()
			.Where(p => (p.StockQuantity ?? 0) <= threshold)
			.ToListAsync();
	}

	public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count)
	{
		if (count <= 0) return Array.Empty<Product>();
		var topProductIds = await _context.OrderItems
			.AsNoTracking()
			.GroupBy(oi => oi.ProductId)
			.Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(x => x.Quantity) })
			.OrderByDescending(x => x.TotalQuantity)
			.Take(count)
			.ToListAsync();

		var ids = topProductIds.Where(x => x.ProductId.HasValue).Select(x => x.ProductId!.Value).ToList();
		var products = await _context.Products.AsNoTracking().Where(p => ids.Contains(p.ProductId)).ToListAsync();
		// Preserve order based on totals
		var orderMap = ids.Select((id, idx) => new { id, idx }).ToDictionary(x => x.id, x => x.idx);
		return products.OrderBy(p => orderMap[p.ProductId]).ToList();
	}
}


