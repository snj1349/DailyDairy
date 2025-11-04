using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IUserRepository
{
	Task<IEnumerable<User>> GetAllAsync();
	Task<User?> GetByIdAsync(int id);
	Task<User> AddAsync(User entity);
	Task<bool> UpdateAsync(User entity);
	Task<bool> DeleteAsync(int id);

	Task<User?> GetByEmailAsync(string email);
	Task<User?> AuthenticateAsync(string email, string passwordHash);
	Task<bool> ChangePasswordAsync(int userId, string newPasswordHash);
}


