using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using iText.Layout;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Events;

namespace FoodDelivery.Areas.Restaurant.Models
{
    public class CommonExcelMethod
    {
        

        public string[] DownloadFile(string webRootPath, string RestaurantName)
        {
            string[] strvalue = null;
            string strFileName = string.Empty;
            string path = System.IO.Path.Combine(System.IO.Path.Combine(webRootPath, "Upload/Pdf/"));
            string filename = string.Empty;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            strFileName = "certificate" + "_" + DateTime.Now.ToString("ddMMyyyyHHMMss") + ".pdf";
            filename = path + "\\" + strFileName;
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);


            }
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);

            PdfWriter pdfWriter = new PdfWriter(fs);

            PdfDocument pdfDocument = new PdfDocument(pdfWriter);

            Document d = new Document(pdfDocument, iText.Kernel.Geom.PageSize.LETTER);
            d.Add(new Paragraph("Certificate"));
            d.Add(new Paragraph(RestaurantName));

            d.Close();
            strvalue = new string[] { strFileName };
            return strvalue;
        } 
    }
}
