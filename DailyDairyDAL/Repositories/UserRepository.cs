using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class UserRepository : IUserRepository
{
	private readonly DailyDairyDbContext _context;

	public UserRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<User>> GetAllAsync()
	{
		return await _context.Users.AsNoTracking().ToListAsync();
	}

	public async Task<User?> GetByIdAsync(int id)
	{
		return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
	}

	public async Task<User> AddAsync(User entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Users.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(User entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Users.AnyAsync(u => u.UserId == entity.UserId);
		if (!exists) return false;
		_context.Users.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
		if (existing == null) return false;
		_context.Users.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		if (string.IsNullOrWhiteSpace(email)) return null;
		return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
	}

	public async Task<User?> AuthenticateAsync(string email, string passwordHash)
	{
		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordHash)) return null;
		return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
	}

	public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
	{
		if (string.IsNullOrWhiteSpace(newPasswordHash)) return false;
		var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
		if (user == null) return false;
		user.PasswordHash = newPasswordHash;
		await _context.SaveChangesAsync();
		return true;
	}
}


