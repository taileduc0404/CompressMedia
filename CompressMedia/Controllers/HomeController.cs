using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CompressMedia.Controllers
{
	public class HomeController : Controller
	{
		private readonly IAuthService _authService;
		private readonly INotyfService _notyfService;

		public HomeController(IAuthService authService, INotyfService notyfService)
		{
			_authService = authService;
			_notyfService = notyfService;
		}

		public IActionResult Index(LoginDto dto)
		{
			return View(dto.Username, dto.Password);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View(new LoginDto());
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto dto)
		{
			if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
			{
				_notyfService.Warning("Please enter both username and password.");
				return View(dto);
			}

			var result = await _authService.Login(dto);

			switch (result)
			{
				case "u":
					_notyfService.Error("Invalid username.");
					return View(new LoginDto());
				case "p":
					_notyfService.Error("Invalid password.");
					return View(new LoginDto());
				default:
					return View("Index", dto);
			}
		}

		[HttpPost]
		public IActionResult Verify(LoginDto loginDto)
		{
			bool result = _authService.VerifyOtp(loginDto);
			if (result)
			{
				_notyfService.Success("Logged in successfully.");
				return RedirectToAction("Index");
			}
			_notyfService.Error("Wrong OTP");
			return RedirectToAction("Index");
		}

		public IActionResult IsLoggedIn()
		{
			if (_authService.IsUserAuthenticated())
			{
				_notyfService.Success("User is logged in");
				return RedirectToAction("Index");
			}
			else
			{
				_notyfService.Error("User is not logged in");
				return RedirectToAction("Index");
			}
		}

		public IActionResult Logout()
		{
			_authService.Logout();
			_notyfService.Success("Logged out successfully.");
			return RedirectToAction("Index");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
