using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;

namespace FoodDelivery.Areas.Restaurant.Controllers
{
    [Area("restaurant")]
    public class DashboardController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DashboardController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        DatabaseRestaurant objDatabaseRestaurant = new DatabaseRestaurant();
        public IActionResult Index(bool IsRestricted = false)
        {
            if (IsRestricted)
            {
                ViewBag.Message = "You are now allowed to access it!";
                ViewBag.MessageType = "danger";
            }
            else {
                var IsCertificateAllow= objDatabaseRestaurant.CheckCertificateIsAllow(GetCurrentRestaurant().RestaurantID);
                ViewBag.IsCertificateAllow = IsCertificateAllow.ToString();
            }
            return View();
        }
        public async Task<ActionResult> GetRestaurantCertificate()
        {
            RestaurantSession restaurantSession=HttpContext.Session.GetComplexData<RestaurantSession>(Common.SessionKeys.RestaurantSession);
            CommonExcelMethod excelMethod = new CommonExcelMethod();
            bool isFileSuccess = true;
            string[] getfile = new string[10];
            getfile = excelMethod.DownloadFile(_hostingEnvironment.WebRootPath, restaurantSession.RestaurantName);
            return Json(new
            {
                FileName = getfile[0],
                IsFileGenerated = isFileSuccess
            });
        }

    }
}
