using FoodDelivery.Areas.AdminUser.Models;
using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodDelivery.Areas.AdminUser.Controllers
{
    [Area("adminuser")]
    public class LoginController : Controller
    {

       
        AdminUserSession restaurantSession = new AdminUserSession();
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult SessionOut()
        {
            return StatusCode(440);
        }

        DatabaseAdminUser objDatabaseAdminUser = new DatabaseAdminUser();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("adminuser")]
        public IActionResult Login(AdminUserLoginModel adminUserLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = "";
                    bool LoginComplete = false;
                    AdminLoginResult adminLoginResult = new AdminLoginResult();

                    adminLoginResult = objDatabaseAdminUser.AdminUserLogin(adminUserLoginModel);

                    if (adminLoginResult.Flag == 1)
                    {
                        result = Common.Messages.UserNotAvailable;
                    }
                    else if (adminLoginResult.Flag == 2)
                    {
                        result = Common.Messages.AdminUserIsNotActive;
                    }
                    else if (adminLoginResult.Flag == 4)
                    {
                        result = Common.Messages.IncorrectPassword;
                    }
                    else if (adminLoginResult.Flag == 5)//successfully login
                    {
                        HttpContext.Session.SetComplexData(Common.SessionKeys.AdminSession, adminLoginResult.AdminUserData);
                    }
                    else
                    {
                        result = Common.Messages.LoginFailed;
                    }
                    return Json(new
                    {
                        status = Convert.ToInt32(adminLoginResult.Flag),
                        message = result
                    });
                }
                else
                {
                    return PartialView(adminUserLoginModel);
                }
            }
            catch (Exception ex) { throw; }
        }

        [HttpPost]
        // [Route("Backoffice/logout")]
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
    }
}
