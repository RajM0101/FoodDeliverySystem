using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodDelivery.Areas.Restaurant.Controllers
{
    [Area("restaurant")]
    public class LoginController : Controller
    {
        private string GetClientIpAddress()
        {
            //var local = HttpContext.Connection.LocalIpAddress?.ToString(); //server IP address - Website hosting server
            var clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedProxyIpAddress))
            {
                clientIpAddress = forwardedProxyIpAddress;
            }
            return Debugger.IsAttached ? "49.36.88.46" : clientIpAddress;
        }
        RestaurantSession restaurantSession = new RestaurantSession();
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult SessionOut()
        {
            return StatusCode(440);
        }

        DatabaseRestaurant objDatabaseRestaurant = new DatabaseRestaurant();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurant")]
        public IActionResult Login(RestaurantLoginModel restaurantLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = "";
                    bool LoginComplete = false;
                    RestaurantLoginResult restaurantLoginResult = new RestaurantLoginResult();

                    restaurantLoginResult = objDatabaseRestaurant.RestaurantLogin(restaurantLoginModel);

                    if (restaurantLoginResult.Flag == 1)
                    {
                        result = Common.Messages.UserNotAvailable;
                    }
                    else if (restaurantLoginResult.Flag == 2)
                    {
                        result = Common.Messages.RestaurantIsNotActive;
                    }
                    else if (restaurantLoginResult.Flag == 4)
                    {
                        result = Common.Messages.IncorrectPassword;
                    }
                    else if (restaurantLoginResult.Flag == 5)//successfully login
                    {
                        HttpContext.Session.SetComplexData(Common.SessionKeys.RestaurantSession, restaurantLoginResult.RestaurantData);
                    }
                    else if (restaurantLoginResult.Flag == 3)
                    {
                        result = Common.Messages.NotApproved;
                    }
                    else
                    {
                        result = Common.Messages.LoginFailed;
                    }
                    return Json(new
                    {
                        status = Convert.ToInt32(restaurantLoginResult.Flag),
                        message = result
                    });
                }
                else
                {
                    return PartialView(restaurantLoginModel);
                }
            }
            catch (Exception ex) { throw; }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                int Status = 0;
                HttpContext.Session.Remove(Common.SessionKeys.RestaurantSession);
                Status = 1;
                return Json(new { statuscode = Status });

            }
            catch (Exception) { throw; }
        }
        [Route("restaurant/sign-up")]
        public IActionResult RestaurantRegistration()
        {

            return View("_RestaurantRegistrationIndex");
        }
        [Route("restaurant/sign-up")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurantRegistration(RestaurantRegistrationModel restaurantRegistrationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var RegistrationResult = objDatabaseRestaurant.RestaurantRegistration(restaurantRegistrationModel);
                    string result = "";


                    return Json(new
                    {
                        status = "200",
                        message = result
                    });
                }
                else
                {
                    return PartialView("_RestaurantRegistrationIndex");
                }
            }
            catch (Exception ex) { throw; }
        }
    }
}
