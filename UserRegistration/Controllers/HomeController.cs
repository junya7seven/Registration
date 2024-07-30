using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserRegistration.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest";

            // Передаем имя пользователя в представление
            ViewData["UserName"] = userName;

            return View();
        }
    }
}
