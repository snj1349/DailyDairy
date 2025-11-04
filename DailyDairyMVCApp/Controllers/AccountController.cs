using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

public class AccountController : Controller
{
	[HttpGet]
	[AllowAnonymous]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[AllowAnonymous]
	public IActionResult Login(string? email, string? password)
	{
		// Placeholder: Implement real login later; for now redirect to Products
		return RedirectToAction("Index", "Products");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return RedirectToAction(nameof(Login));
	}

	[HttpGet]
	[AllowAnonymous]
	public IActionResult AccessDenied()
	{
		return View();
	}
}


