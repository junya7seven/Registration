using DataSource;
using DataSource.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace UserRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private IUserChangeService _userChangeService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IUserRepository userRepository, IUserChangeService userChangeService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userService = userService;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _userChangeService = userChangeService;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Guest";

            ViewData["UserName"] = userName;

            return View();
        }

        [HttpGet]
        public IActionResult Rename()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        public async Task<IActionResult> Rename(string NewLogin)
        {
            await _userChangeService.UserRaname(NewLogin);
            var user = _userRepository.SearchByLogin(NewLogin);

            return Ok(user);
        }
    }
}
