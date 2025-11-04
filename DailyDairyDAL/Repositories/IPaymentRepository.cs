using System.Collections.Generic;
using System.Threading.Tasks;
using DailyDairyDAL.Models;

namespace DailyDairyDAL.Repositories;

public interface IPaymentRepository
{
	Task<IEnumerable<Payment>> GetAllAsync();
	Task<Payment?> GetByIdAsync(int id);
	Task<Payment> AddAsync(Payment entity);
	Task<bool> UpdateAsync(Payment entity);
	Task<bool> DeleteAsync(int id);

	Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
	Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
	Task<IEnumerable<Payment>> GetRecentPaymentsAsync(int count);
}


