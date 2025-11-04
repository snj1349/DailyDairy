using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

public class ProductsController : Controller
{
	private readonly IProductRepository _products;
	private readonly ICategoryRepository _categories;

	public ProductsController(IProductRepository products, ICategoryRepository categories)
	{
		_products = products;
		_categories = categories;
	}

	public async Task<IActionResult> Index()
	{
		var items = await _products.GetAllAsync();
		return View(items);
	}

	public async Task<IActionResult> Details(int id)
	{
		var item = await _products.GetByIdAsync(id);
		if (item == null) return NotFound();
		return View(item);
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Create()
	{
		ViewBag.Categories = await _categories.GetAllAsync();
		return View();
	}

	[HttpPost]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Create(Product model)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.Categories = await _categories.GetAllAsync();
			return View(model);
		}
		await _products.AddAsync(model);
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id)
	{
		var item = await _products.GetByIdAsync(id);
		if (item == null) return NotFound();
		ViewBag.Categories = await _categories.GetAllAsync();
		return View(item);
	}

	[HttpPost]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Edit(int id, Product model)
	{
		if (id != model.ProductId) return BadRequest();
		if (!ModelState.IsValid)
		{
			ViewBag.Categories = await _categories.GetAllAsync();
			return View(model);
		}
		var updated = await _products.UpdateAsync(model);
		if (!updated) return NotFound();
		return RedirectToAction(nameof(Index));
	}

	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> Delete(int id)
	{
		var item = await _products.GetByIdAsync(id);
		if (item == null) return NotFound();
		return View(item);
	}

	[HttpPost, ActionName("Delete")]
	[Authorize(Roles = "Admin,Owner")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		await _products.DeleteAsync(id);
		return RedirectToAction(nameof(Index));
	}
}


