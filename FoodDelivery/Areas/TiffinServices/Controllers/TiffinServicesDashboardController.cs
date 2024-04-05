using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Areas.TiffinServices.Controllers;
using FoodDelivery.Areas.TiffinServices.Models;
using FoodDelivery.Models;

namespace FoodDelivery.Areas.TiffinServices.Controllers
{
    [Area("tiffinservices")]
    public class TiffinServicesDashboardController : TiffinServicesBaseController
    {
        DatabaseTiffinServices objDatabaseTiffinServices = new DatabaseTiffinServices();
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TiffinServicesDashboardController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index(bool IsRestricted = false)
        {
            if (IsRestricted)
            {
                ViewBag.Message = "You are now allowed to access it!";
                ViewBag.MessageType = "danger";
            }
            else {
                var IsCertificateAllow = objDatabaseTiffinServices.CheckCertificateIsAllow(GetCurrentTiffinServices().TiffinServicesID);
                ViewBag.IsCertificateAllow = IsCertificateAllow.ToString();
            }

            return View();
        }
        public async Task<ActionResult> GetTiffinServicesCertificate()
        {
            TiffinServicesSession restaurantSession = HttpContext.Session.GetComplexData<TiffinServicesSession>(Common.SessionKeys.TiffinServicesSession);
            TiffinServicesCommonExcelMethod excelMethod = new TiffinServicesCommonExcelMethod();
            bool isFileSuccess = true;
            string[] getfile = new string[10];
            getfile = excelMethod.DownloadFile(_hostingEnvironment.WebRootPath, restaurantSession.TiffinServicesName);
            return Json(new
            {
                FileName = getfile[0],
                IsFileGenerated = isFileSuccess
            });
        }
    }
}
