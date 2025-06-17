using Employee.BusinessLogic.Interfaces.Service.Security;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.UI.Models;
using Employee_UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace Employee_UI.Controllers
{
    [EnableRateLimiting("fixed")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly ICryptographyService _cryptographyService;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, ICryptographyService cryptographyService)
        {
            _logger = logger;
            _context = appDbContext;
            _cryptographyService = cryptographyService;
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

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginViewModel.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginViewModel);
            }

            var verfiedPassword = _cryptographyService.VerifyPassword(loginViewModel.Password, user.Password);

            if(!verfiedPassword)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginViewModel);
            }

            var userBuilder = new StringBuilder();
            userBuilder.Append(user.FirstName).Append(' ').Append(user.LastName);
            HttpContext.Session.SetString("userName", userBuilder.ToString());
            HttpContext.Session.SetString("LoggedInAt", DateTime.Now.ToString());
            userBuilder.Clear();
            return RedirectToAction("Index", "Employees");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            ViewBag.UserName = HttpContext.Session.GetString("userName");

            var loggedInAt = DateTime.TryParse(HttpContext.Session.GetString("LoggedInAt"), out var loggedIn);
            var currentTime = DateTime.Now;
            var timeSpan = loggedInAt ? currentTime - loggedIn : TimeSpan.Zero;
            ViewBag.TimeSpan = timeSpan.ToString(@"mm\:ss");
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
