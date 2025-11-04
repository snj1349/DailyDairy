using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

[Authorize]
public class OrdersController : Controller
{
	private readonly IOrderRepository _orders;
    private readonly IUserRepository _users;

	public OrdersController(IOrderRepository orders, IUserRepository users)
	{
		_orders = orders;
		_users = users;
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Index()
	{
		var list = await _orders.GetAllAsync();
		return View(list);
	}

	public async Task<IActionResult> Details(int id)
	{
		var entity = await _orders.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[Authorize(Roles = "Customer,Owner,Admin")]
	public IActionResult Create() => View();

	[HttpPost]
	[Authorize(Roles = "Customer,Owner,Admin")]
	public async Task<IActionResult> Create(Order model)
	{
		if (!ModelState.IsValid) return View(model);
		await _orders.AddAsync(model);
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id)
	{
		var entity = await _orders.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id, Order model)
	{
		if (id != model.OrderId) return BadRequest();
		if (!ModelState.IsValid) return View(model);
		var ok = await _orders.UpdateAsync(model);
		if (!ok) return NotFound();
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Delete(int id)
	{
		var entity = await _orders.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost, ActionName("Delete")]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _orders.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}

	// Special actions
	[Authorize(Roles = "Customer,Owner,Admin")]
	public async Task<IActionResult> PlaceOrder()
	{
		var currentUser = await GetCurrentUserAsync();
		if (currentUser == null) return Unauthorized();
		var order = new Order { UserId = currentUser.UserId, Status = "Pending" };
		var created = await _orders.AddAsync(order);
		return RedirectToAction(nameof(Details), new { id = created.OrderId });
	}

	public async Task<IActionResult> OrderHistory()
	{
		var currentUser = await GetCurrentUserAsync();
		if (currentUser == null) return Unauthorized();
		var list = await _orders.GetOrdersByUserAsync(currentUser.UserId);
		return View(list);
	}

	private async Task<User?> GetCurrentUserAsync()
	{
		var email = User.FindFirstValue(ClaimTypes.Email);
		if (string.IsNullOrWhiteSpace(email)) return null;
		return await _users.GetByEmailAsync(email);
	}
}


