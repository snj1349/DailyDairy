using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IReviewRepository
{
	Task<IEnumerable<Review>> GetAllAsync();
	Task<Review?> GetByIdAsync(int id);
	Task<Review> AddAsync(Review entity);
	Task<bool> UpdateAsync(Review entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId);
	Task<double?> GetAverageRatingByProductAsync(int productId);
	Task<Review?> GetUserReviewForProductAsync(int userId, int productId);
}


