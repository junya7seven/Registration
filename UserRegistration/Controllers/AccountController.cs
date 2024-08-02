using DataSource;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Models;
using System.Security.Claims;
using DataSource.Model;

namespace UserRegistration.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userService = userService;
            _logger.LogDebug(1, "NLog injected into InfoController");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "User validation failed");
                Response.StatusCode = 400;
                return View(model);
            }
            if (model.Login == model.Email)
            {
                ModelState.AddModelError(model.Login, "Login and Email must not match ");
            }
            if (!await _userService.UserValidation(model))
            {
                Response.StatusCode = 400;
                ModelState.AddModelError(string.Empty, "User validation failed");
                return View(model);
            }
            var result = await _userService.RegisterUser(model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "The username or password is already registered");
                return View(model);
            }
            Response.StatusCode = 200;
            return RedirectToAction("Login");
        }



        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await _userService.AuthenticateUser(model);
            if (token == null)
            {
                Response.StatusCode = 401;
                ModelState.AddModelError(string.Empty, "Invalid login or password");
                return View(model);
            }

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true 
            });

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Login)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true }
            );
            Response.StatusCode = 200;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            Response.Cookies.Delete("jwt");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
    
}
