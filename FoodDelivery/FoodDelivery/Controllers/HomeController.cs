using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("login-register", Name = "UserLoginRegister")]
        [HttpGet]
        public ActionResult UserLoginRegister()
        {
            LoginRegisterModel model = new LoginRegisterModel();
            return PartialView("_Login", model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    #region ViewComponent Section
    public class RegisterUserViewComponent : ViewComponent
    {
        Database objDatabase = new Database();
        public async Task<IViewComponentResult> InvokeAsync(RegisterUserModel registerUserModel)
        {
            return View("RegisterUser", registerUserModel);
        }
    }

    public class LoginUserViewComponent : ViewComponent
    {
        Database objDatabase = new Database();
        public async Task<IViewComponentResult> InvokeAsync(LoginUserModel? loginUserModel)
        {
            return View("LoginUser", loginUserModel);
        }
    }

    #endregion
}