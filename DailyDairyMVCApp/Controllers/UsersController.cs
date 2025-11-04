using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
	private readonly IUserRepository _users;
	private readonly IPasswordHasher<User> _hasher;

	public UsersController(IUserRepository users, IPasswordHasher<User> hasher)
	{
		_users = users;
		_hasher = hasher;
	}

	public async Task<IActionResult> Index()
	{
		var list = await _users.GetAllAsync();
		return View(list);
	}

	public async Task<IActionResult> Details(int id)
	{
		var entity = await _users.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	public IActionResult Create() => View();

	[HttpPost]
	public async Task<IActionResult> Create(User model, string password)
	{
		// PasswordHash will be set after hashing the provided password; exclude it from validation
		ModelState.Remove(nameof(DailyDairyDAL.Models.User.PasswordHash));
		if (!ModelState.IsValid) return View(model);
		model.PasswordHash = _hasher.HashPassword(model, password);
		await _users.AddAsync(model);
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var entity = await _users.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, User model)
	{
		if (id != model.UserId) return BadRequest();
		if (!ModelState.IsValid) return View(model);
		var ok = await _users.UpdateAsync(model);
		if (!ok) return NotFound();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var entity = await _users.GetByIdAsync(id);
		if (entity == null) return NotFound();
		return View(entity);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _users.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}
}


