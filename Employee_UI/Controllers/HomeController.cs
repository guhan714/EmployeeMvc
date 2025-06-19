using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Employee_UI.Models;
using Employee.BusinessLogic.Interfaces.Service.Domain;
using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.BusinessLogic.Interfaces.Service.Security;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Employee.UI.Controllers
{
    [EnableRateLimiting("fixed")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly ILoginService _loginService;
        private readonly IRoleService _roleService;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, ICryptographyService cryptographyService, ILoginService loginService, IRoleService roleService)
        {
            _logger = logger;
            _context = appDbContext;
            _loginService = loginService;
            _roleService = roleService;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            
            var user = await _loginService.LoginAsync(loginViewModel.Email, loginViewModel.Password);

            if (!user.IsSuccess)
            {
                ModelState.AddModelError("", user.Message);
                return View(loginViewModel);
            }

            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, user?.Data?.UserId.ToString() ?? string.Empty),
                new(ClaimTypes.Name, user.Data.Username ?? string.Empty),
                new(ClaimTypes.Email, loginViewModel.Email),
            ];

            var roles = await _roleService.GetUserRolesAsync(user.Data.UserId); // ← Create this method
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.RoleName)); // Assign only this user's roles
            }


            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };
            
            await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties);
            
            HttpContext.Session.SetString("userName", user.Data?.Username ?? string.Empty);
            HttpContext.Session.SetString("LoggedInAt", DateTime.Now.ToString()); // ISO 8601, consistent
            return RedirectToAction("Index", "Employees");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            ViewBag.UserName = HttpContext.Session.GetString("userName");

            var loggedInAt = DateTime.TryParse(HttpContext.Session.GetString("LoggedInAt"), out var loggedIn);
            var currentTime = DateTime.Now;
            var timeSpan = loggedInAt ? currentTime - loggedIn : TimeSpan.Zero;
            ViewBag.TimeSpan = timeSpan.ToString(@"mm\:ss");
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("MyCookieAuth");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
