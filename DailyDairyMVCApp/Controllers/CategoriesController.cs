using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

[Authorize(Roles = "Admin,Owner")]
public class CategoriesController : Controller
{
	private readonly ICategoryRepository _categories;

	public CategoriesController(ICategoryRepository categories)
	{
		_categories = categories;
	}

	[AllowAnonymous]
	public async Task<IActionResult> Index()
	{
		var items = await _categories.GetAllAsync();
		return View(items);
	}

	[AllowAnonymous]
	public async Task<IActionResult> Details(int id)
	{
		var item = await _categories.GetByIdAsync(id);
		if (item == null) return NotFound();
		return View(item);
	}

	public IActionResult Create() => View();

	[HttpPost]
	public async Task<IActionResult> Create(Category model)
	{
		if (!ModelState.IsValid) return View(model);
		await _categories.AddAsync(model);
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Edit(int id)
	{
		var item = await _categories.GetByIdAsync(id);
		if (item == null) return NotFound();
		return View(item);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(int id, Category model)
	{
		if (id != model.CategoryId) return BadRequest();
		if (!ModelState.IsValid) return View(model);
		var ok = await _categories.UpdateAsync(model);
		if (!ok) return NotFound();
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Delete(int id)
	{
		var item = await _categories.GetByIdAsync(id);
		if (item == null) return NotFound();
		return View(item);
	}

	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _categories.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}
}


