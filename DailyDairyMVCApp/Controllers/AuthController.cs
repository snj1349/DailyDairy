using DailyDairyDAL.Models;
using DailyDairyDAL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DailyDairyMVCApp.Controllers;

public class AuthController : Controller
{
	private readonly IUserRepository _users;
	private readonly IPasswordHasher<User> _hasher;

	public AuthController(IUserRepository users, IPasswordHasher<User> hasher)
	{
		_users = users;
		_hasher = hasher;
	}

	[HttpGet]
	public IActionResult Login() => View();

	[HttpPost]
	public async Task<IActionResult> Login(string email, string password)
	{
		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			ModelState.AddModelError(string.Empty, "Email and password are required.");
			return View();
		}

		var user = await _users.GetByEmailAsync(email);
		if (user == null)
		{
			ModelState.AddModelError(string.Empty, "Invalid credentials.");
			return View();
		}

		var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
		if (verify == PasswordVerificationResult.Failed)
		{
			ModelState.AddModelError(string.Empty, "Invalid credentials.");
			return View();
		}

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
			new Claim(ClaimTypes.Name, user.FullName),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role ?? "Customer")
		};
		var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

		return RedirectToAction("Index", "Products");
	}

	[HttpGet]
	public IActionResult Register() => View();

	[HttpPost]
	public async Task<IActionResult> Register(User model, string password)
	{
		// PasswordHash is set server-side after hashing; exclude from validation
		ModelState.Remove(nameof(DailyDairyDAL.Models.User.PasswordHash));
		if (!ModelState.IsValid)
		{
			return View(model);
		}
		var existing = await _users.GetByEmailAsync(model.Email);
		if (existing != null)
		{
			ModelState.AddModelError(string.Empty, "Email already registered.");
			return View(model);
		}
		model.PasswordHash = _hasher.HashPassword(model, password);
		model.Role = string.IsNullOrWhiteSpace(model.Role) ? "Customer" : model.Role;
		await _users.AddAsync(model);
		return RedirectToAction("Login");
	}

	[HttpPost]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		return RedirectToAction("Login");
	}
}


