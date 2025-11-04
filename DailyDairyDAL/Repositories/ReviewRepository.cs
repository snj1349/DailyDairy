using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class ReviewRepository : IReviewRepository
{
	private readonly DailyDairyDbContext _context;

	public ReviewRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Review>> GetAllAsync()
	{
		return await _context.Reviews.AsNoTracking().ToListAsync();
	}

	public async Task<Review?> GetByIdAsync(int id)
	{
		return await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.ReviewId == id);
	}

	public async Task<Review> AddAsync(Review entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Reviews.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Review entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Reviews.AnyAsync(r => r.ReviewId == entity.ReviewId);
		if (!exists) return false;
		_context.Reviews.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id);
		if (existing == null) return false;
		_context.Reviews.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId)
	{
		return await _context.Reviews.AsNoTracking().Where(r => r.ProductId == productId).ToListAsync();
	}

	public async Task<double?> GetAverageRatingByProductAsync(int productId)
	{
		return await _context.Reviews.AsNoTracking().Where(r => r.ProductId == productId).Select(r => (double?)r.Rating).AverageAsync();
	}

	public async Task<Review?> GetUserReviewForProductAsync(int userId, int productId)
	{
		return await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
	}
}


