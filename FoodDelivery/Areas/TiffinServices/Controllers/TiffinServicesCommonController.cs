
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Areas.TiffinServices.Controllers
{
    [Area("tiffinservices")]
    public class TiffinServicesCommonController : TiffinServicesBaseController
    {
        [Route("tiffinservices/downloadpdf")]
        public FileResult DownloadPdf(string FileName)
        {
            string fullPath = Path.Combine(("~/Upload/Pdf/" + FileName));
            return File(fullPath, "application/pdf", FileName);
        }
    }
}
