using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Areas.AdminUser.Controllers
{
    [Area("adminuser")]
    public class DashboardController : AdminBaseController
    {
        public IActionResult Index(bool IsRestricted = false)
        {
            if (IsRestricted)
            {
                ViewBag.Message = "You are now allowed to access it!";
                ViewBag.MessageType = "danger";
            }

            return View();
        }
    }
}
