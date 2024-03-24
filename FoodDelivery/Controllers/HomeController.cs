using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

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
            DashboardMainModel dashboardMainModel = GetDashboardAllDetails();
            return View(dashboardMainModel);
        }
        [Route("/tiffin-services", Name = "TiffinPage")]
        public ActionResult TiffinServices()
        {
            DashboardMainModel dashboardMainModel = GetDashboardAllDetails();
            return View(dashboardMainModel);
        }
        [Route("/restaurants", Name = "RestaurantsPage")]
        public ActionResult Restaurants()
        {
            DashboardMainModel dashboardMainModel = GetDashboardAllDetails();
            return View(dashboardMainModel);
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
                HttpContext.Session.SetComplexData(Common.SessionKeys.UserSession, new SessionUser { FullName = loginUser.SessionUser.FullName, UserId = loginUser.SessionUser.UserId, MobileNumber = loginUser.SessionUser.MobileNumber });
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
            dashboardMainModel.restaurantDetailModel = objDatabase.GetAllRestaurantDetail();
            return dashboardMainModel;
        }
        [Route("/restaurants-tiffin/{RestaurantID?}", Name = "GetFoodList")]
        public ActionResult GetFoodList(int RestaurantID)
        {
            FoodListbyRestaurantIdMainModel MainModel = new FoodListbyRestaurantIdMainModel();
            MainModel = objDatabase.GetFoodListByRestaurant(RestaurantID);

            return View("_FoodList", MainModel);
        }

        [Route("/food-detail/{FoodID?}", Name = "FoodDetails")]
        public ActionResult FoodDetails(int FoodID)
        {
            GetFoodDetailsById getFoodDetailsById = new GetFoodDetailsById();
            getFoodDetailsById = objDatabase.GetFoodItemDetailsById(FoodID);
            return View("_FoodDetails", getFoodDetailsById);
        }
        [Route("/search-food-restaurant", Name = "SearchFoodRestaurant")]
        public ActionResult SearchRestaurantAndFood(string SearchBy)
        {
            SearchFoodAndRestaurant searchFoodAndRestaurant = new SearchFoodAndRestaurant();
            searchFoodAndRestaurant = objDatabase.GetSearchRestaurantAndFood(SearchBy);
            return View("_SearchView", searchFoodAndRestaurant);
        }

        [Route("/all-foodlist", Name = "AllFoodList")]
        public ActionResult AllFoodList(bool IsVegFood,bool IsJainFood,bool IsRatingCheck,decimal PriceMinVal=0, decimal PriceMaxVal=500)
        {
            FoodListbyRestaurantIdMainModel MainModel = new FoodListbyRestaurantIdMainModel();
            MainModel = objDatabase.GetFoodListByRestaurant(IsVegFood, IsJainFood, IsRatingCheck, PriceMinVal, PriceMaxVal);
            return View("_AllFoodList", MainModel);
        }
        public ActionResult GetAllFoodList(bool IsVegFood, bool IsJainFood, bool IsRatingCheck,decimal PriceMinVal=0,decimal PriceMaxVal=500)
        {
            FoodListbyRestaurantIdMainModel MainModel = new FoodListbyRestaurantIdMainModel();
            MainModel = objDatabase.GetFoodListByRestaurant(IsVegFood, IsJainFood, IsRatingCheck,PriceMinVal,PriceMaxVal);
            return ViewComponent("PartialFoodList", MainModel);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/error/{statusCode?}", Name = "ErrorStatusCode")]
        public IActionResult ErrorStatusCode(string statusCode)
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            //https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-7.0
            return View(new ErrorStatusCodeViewModel { StatusCode = statusCode });
        }
        public IActionResult SessionOut()
        {
            return StatusCode(440);
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
    public class PartialFoodListViewComponent : ViewComponent
    {
        Database objDatabase = new Database();
        public async Task<IViewComponentResult> InvokeAsync(FoodListbyRestaurantIdMainModel? Model)
        {
            return View("PartialFoodList", Model);
        }
    }
    #endregion
}