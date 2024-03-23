using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Areas.TiffinServices.Controllers;

namespace FoodDelivery.Areas.TiffinServices.Controllers
{
    [Area("tiffinservices")]
    public class TiffinServicesDashboardController : TiffinServicesBaseController
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
