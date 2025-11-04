using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

[Authorize]
public class PaymentsController : Controller
{
	private readonly IPaymentRepository _payments;

	public PaymentsController(IPaymentRepository payments)
	{
		_payments = payments;
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Index()
	{
		var list = await _payments.GetAllAsync();
		return View(list);
	}

	public async Task<IActionResult> Details(int id)
	{
		var entity = await _payments.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[Authorize(Roles = "Admin,Owner")]
	public IActionResult Create() => View();

	[HttpPost]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Create(Payment model)
	{
		if (!ModelState.IsValid) return View(model);
		await _payments.AddAsync(model);
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id)
	{
		var entity = await _payments.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id, Payment model)
	{
		if (id != model.PaymentId) return BadRequest();
		if (!ModelState.IsValid) return View(model);
		var ok = await _payments.UpdateAsync(model);
		if (!ok) return NotFound();
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Delete(int id)
	{
		var entity = await _payments.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost, ActionName("Delete")]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _payments.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}

	// Special action
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> ProcessPayment(int id, string status)
	{
		var ok = await _payments.UpdatePaymentStatusAsync(id, status);
		if (!ok) return NotFound();
		return RedirectToAction(nameof(Details), new { id });
	}
}


