using Microsoft.AspNetCore.Mvc;

namespace CompressMedia.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(AccessDenied));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
