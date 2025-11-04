using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyDairyDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyDairyDAL.Repositories;

public class PaymentRepository : IPaymentRepository
{
	private readonly DailyDairyDbContext _context;

	public PaymentRepository(DailyDairyDbContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Payment>> GetAllAsync()
	{
		return await _context.Payments.AsNoTracking().ToListAsync();
	}

	public async Task<Payment?> GetByIdAsync(int id)
	{
		return await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.PaymentId == id);
	}

	public async Task<Payment> AddAsync(Payment entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		await _context.Payments.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(Payment entity)
	{
		if (entity == null) throw new ArgumentNullException(nameof(entity));
		var exists = await _context.Payments.AnyAsync(p => p.PaymentId == entity.PaymentId);
		if (!exists) return false;
		_context.Payments.Update(entity);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var existing = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
		if (existing == null) return false;
		_context.Payments.Remove(existing);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId)
	{
		return await _context.Payments
			.Include(p => p.Order)
			.AsNoTracking()
			.Where(p => p.Order != null && p.Order.UserId == userId)
			.ToListAsync();
	}

	public async Task<bool> UpdatePaymentStatusAsync(int paymentId, string status)
	{
		if (string.IsNullOrWhiteSpace(status)) return false;
		var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
		if (payment == null) return false;
		payment.Status = status;
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<IEnumerable<Payment>> GetRecentPaymentsAsync(int count)
	{
		if (count <= 0) return Array.Empty<Payment>();
		return await _context.Payments.AsNoTracking()
			.OrderByDescending(p => p.PaymentDate)
			.Take(count)
			.ToListAsync();
	}
}


