using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

namespace FoodDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Database objDatabase = new Database();
        SessionUser sessionUser = new SessionUser();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Session.GetComplexData<SessionUser>(Common.SessionKeys.UserSession) == null)
            {
                
            }

            base.OnActionExecuting(filterContext);
        }
        [Route("/", Name = "HomePage")]
        public ActionResult Index()
        {
            //DashboardMainModel dashboardMainModel = GetDashboardAllDetails();
            //return View(dashboardMainModel);
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
        [Route("login-register", Name = "UserLoginRegister")]
        [HttpPost]
        public ActionResult UserLogin(LoginUserModel loginUserModel)
        {

            string OTP = string.Empty;
            var loginUser = objDatabase.UserLogin(loginUserModel);

            string Message = string.Empty;

            if (loginUser.RetStatus == 1)
            {
                //login successfully
                HttpContext.Session.SetComplexData(Common.SessionKeys.UserSession, new SessionUser {FullName = loginUser.SessionUser.FullName, UserId = loginUser.SessionUser.UserId, MobileNumber = loginUser.SessionUser.MobileNumber });
                TempData["Message"] = Common.Messages.CustomerLoginSucessfully;
                TempData["MessageType"] = "success";
                return Json(new { IsLoginCompleted = true });

            }
            else if (loginUser.RetStatus == 2)
            {
                //Password or username is uncorrect
                ModelState.AddModelError("IncorrectPassword", Common.Messages.CustomerPasswordIncorrect);
                return ViewComponent("LoginUser", loginUserModel);
            }
            else if (loginUser.RetStatus == 3)
            {
                //Password or username is uncorrect
                ModelState.AddModelError("CustomerIsNotActive", Common.Messages.CustomerIsNotActive);
                return ViewComponent("LoginUser", loginUserModel);
            }
            else
            {
                //user not found
                ModelState.AddModelError("UserIsNotValid", Common.Messages.CustomerNotAvailable);
                return ViewComponent("LoginUser", loginUserModel);
            }

        }

        [HttpPost]
        public IActionResult UserRegister(RegisterUserModel registerUserModel)
        {
            try
            {
                string result = "";
                if (ModelState.IsValid)
                {
                    if (registerUserModel.TermsCondition == false)
                    {
                        return ViewComponent("RegisterUser", registerUserModel);
                    }
                    else
                    {
                        var RegistrationResult = objDatabase.UserRegister(registerUserModel);
                        if (RegistrationResult.RetStatus == 1)
                        {
                            return Json(new
                            {
                                status = "200",
                                message = result
                            });
                        }
                        if (RegistrationResult.RetStatus == 2)
                        {
                            ModelState.AddModelError("UserAlreadyExists", Common.Messages.UserAlreadyRegistered);
                           
                        }
                    }
                    return ViewComponent("RegisterUser", registerUserModel);
                }
                else
                {
                    return ViewComponent("RegisterUser", registerUserModel);
                }
            }
            catch (Exception ex) { throw; }
        }

        #region User Logout

        [Route("logout", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove(Common.SessionKeys.UserSession);
                return RedirectToAction("Index");
            }
            catch (Exception) { throw; }
        }
        #endregion

        public DashboardMainModel GetDashboardAllDetails()
        {
            DashboardMainModel dashboardMainModel = new DashboardMainModel();
            dashboardMainModel.restaurantDetailModel= objDatabase.GetAllRestaurantDetail();
            return dashboardMainModel;
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