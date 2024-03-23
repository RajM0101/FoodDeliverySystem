using FoodDelivery.Areas.TiffinServices.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodDelivery.Areas.TiffinServices.Controllers
{
    [Area("tiffinservices")]
    public class TiffinServicesLoginController : Controller
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
        TiffinServicesSession tiffinServicesSession = new TiffinServicesSession();
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult SessionOut()
        {
            return StatusCode(440);
        }

        DatabaseTiffinServices objDatabaseTiffinServices = new DatabaseTiffinServices();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("tiffinservices")]
        public IActionResult Login(TiffinServicesLoginModel tiffinServicesLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = "";
                    bool LoginComplete = false;
                    TiffinServicesLoginResult tiffinServicesLoginResult = new TiffinServicesLoginResult();

                    tiffinServicesLoginResult = objDatabaseTiffinServices.TiffinServicesLogin(tiffinServicesLoginModel);

                    if (tiffinServicesLoginResult.Flag == 1)
                    {
                        result = Common.Messages.UserNotAvailable;
                    }
                    else if (tiffinServicesLoginResult.Flag == 2)
                    {
                        result = Common.Messages.TiffinServicesIsNotActive;
                    }
                    else if (tiffinServicesLoginResult.Flag == 4)
                    {
                        result = Common.Messages.IncorrectPassword;
                    }
                    else if (tiffinServicesLoginResult.Flag == 5)//successfully login
                    {
                        HttpContext.Session.SetComplexData(Common.SessionKeys.TiffinServicesSession, tiffinServicesLoginResult.TiffinServicesData);
                    }
                    else if (tiffinServicesLoginResult.Flag == 3)
                    {
                        result = Common.Messages.NotApproved;
                    }
                    else
                    {
                        result = Common.Messages.LoginFailed;
                    }
                    return Json(new
                    {
                        status = Convert.ToInt32(tiffinServicesLoginResult.Flag),
                        message = result
                    });
                }
                else
                {
                    return PartialView(tiffinServicesLoginModel);
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
                HttpContext.Session.Remove(Common.SessionKeys.TiffinServicesSession);
                Status = 1;
                return Json(new { statuscode = Status });

            }
            catch (Exception) { throw; }
        }
        [Route("tiffinservices/sign-up")]
        public IActionResult TiffinServicesRegistration()
        {

            return View("_TiffinServicesRegistrationIndex");
        }
        [Route("tiffinservices/sign-up")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TiffinServicesRegistration(TiffinServicesRegistrationModel tiffinServicesRegistrationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var RegistrationResult = objDatabaseTiffinServices.TiffinServicesRegistration(tiffinServicesRegistrationModel);
                    string result = "";


                    return Json(new
                    {
                        status = "200",
                        message = result
                    });
                }
                else
                {
                    return PartialView("_TiffinServicesRegistrationIndex");
                }
            }
            catch (Exception ex) { throw; }
        }
    }
}
