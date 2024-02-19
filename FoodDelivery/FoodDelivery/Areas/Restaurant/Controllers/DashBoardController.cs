using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
namespace FoodDelivery.Areas.Restaurant.Controllers
{
    [Area("restaurant")]
    public class DashboardController : BaseController
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
