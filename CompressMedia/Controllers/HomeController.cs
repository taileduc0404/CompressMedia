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
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly INotyfService _notyfService;

        public HomeController(IAuthService authService, ILogger<HomeController> logger, INotyfService notyfService)
        {
            _authService = authService;
            _logger = logger;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Warning("Please enter your info.");
                return View(nameof(Register));
            }
            var result = await _authService.Register(dto);

            switch (result)
            {
                case "null":
                    _notyfService.Error("Register failed.");
                    return View(nameof(Register));
                case "usernameExist":
                    _notyfService.Warning("Username you enter already exist.");
                    return View(nameof(Register));
                default:
                    _notyfService.Success("Register successfully.");
                    return RedirectToAction("Index");
            }


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
                return View(nameof(Login));
            }

            var token = await _authService.Login(dto);

            switch (token)
            {
                case "u":
                    _notyfService.Error("Invalid username.");
                    return View(nameof(Login));
                case "p":
                    _notyfService.Error("Invalid password.");
                    return View(nameof(Login));
                default:
                    _notyfService.Success("Logged in successfully.");
                    return RedirectToAction("Index");
            }
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
