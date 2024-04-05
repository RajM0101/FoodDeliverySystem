using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Areas.Restaurant.Controllers
{
    [Area("restaurant")]
    public class CommonController : BaseController
    {
        [Route("restaurant/downloadpdf")]
        public FileResult DownloadPdf(string FileName)
        {
            string fullPath = Path.Combine(("~/Upload/Pdf/" + FileName));
            return File(fullPath, "application/pdf", FileName);
        }
    }
}
