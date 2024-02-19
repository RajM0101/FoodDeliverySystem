using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace FoodDelivery.Areas.Restaurant.Controllers
{
    [Area("restaurant")]
    public class FoodController : BaseController
    {
        private readonly string _imageFolderPath;
        private readonly IWebHostEnvironment _env;
        private readonly IWebHostEnvironment _hosting;

        public FoodController(IWebHostEnvironment hosting, IWebHostEnvironment env)
        {
            _hosting = hosting;
            _env = env;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images","Food");
            Directory.CreateDirectory(_imageFolderPath);
        }
        
        DatabaseRestaurant objDatabaseRestaurant = new DatabaseRestaurant();
        public void GetBestSeller()
        {
            List<SelectListItem> IsBestSeller = new List<SelectListItem>();
            IsBestSeller.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsBestSeller.Add(new SelectListItem { Value = "1", Text = "BestSeller" });
            IsBestSeller.Add(new SelectListItem { Value = "0", Text = "Regular" });
            ViewBag.IsBestSeller = new SelectList(IsBestSeller, "Value", "Text");
        }
        public void GetVegetarian()
        {
            List<SelectListItem> IsVegetarian = new List<SelectListItem>();
            IsVegetarian.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsVegetarian.Add(new SelectListItem { Value = "1", Text = "Vegetarian" });
            IsVegetarian.Add(new SelectListItem { Value = "0", Text = "Non Vegetarian" });
            ViewBag.IsVegetarian = new SelectList(IsVegetarian, "Value", "Text");
        }
        public void GetActiveInActive()
        {
            List<SelectListItem> IsAvailable = new List<SelectListItem>();
            IsAvailable.Add(new SelectListItem { Value = "-1", Text = "-- All --" });
            IsAvailable.Add(new SelectListItem { Value = "1", Text = "Available" });
            IsAvailable.Add(new SelectListItem { Value = "0", Text = "Unavilable" });
            ViewBag.IsAvailable = new SelectList(IsAvailable, "Value", "Text");
        }


        [Route("restaurant/food-item")]
        public ActionResult FoodItemList()
        {
            GetBestSeller();
            GetVegetarian();
            GetActiveInActive();
            return View("_FoodItemList");
        }
        [HttpGet]
        public ActionResult GetFoodItemList(JQueryDataTableParamModel param, string Name, string DiscountInPercentage, string IsAvailable, string IsVegetarian, string IsBestSeller)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<FoodItemListModel> list = objDatabaseRestaurant.GetFoodItemList(param, Name, DiscountInPercentage, IsAvailable, IsVegetarian, IsBestSeller, GetCurrentRestaurant().RestaurantID, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.FoodID),
                        c.FoodName,
                        Convert.ToString(c.Price),
                        Convert.ToString(c.DiscountInPercentage)+"%",
                        (c.IsJainAvailable==true ?"Jain Available":"No Jain"),
                        c.IsBestSeller==true ?"Yes":"No",
                        c.IsVegetarian==true ?"Veg":"Non-Veg",
                        c.IsAvailable==true ?"Yes":"No"
                      };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = noOfRecords,
                    iTotalDisplayRecords = noOfRecords,
                    aaData = obj
                });
            }
            catch (Exception) { throw; }
        }


        [Route("restaurant/add-edit-food")]
        [HttpGet]
        public ActionResult AddEditFood(int FoodId)
        {
            AddEditFoodViewModel addEditfoodModel = new AddEditFoodViewModel();

            GetFoodDetailsById getPackageDetailsById = objDatabaseRestaurant.GetFoodItemDetailsById(FoodId, GetCurrentRestaurant().RestaurantID);
            if (getPackageDetailsById != null)
            {
                addEditfoodModel.FoodId = FoodId;
                addEditfoodModel.Name = getPackageDetailsById.FoodName;
                addEditfoodModel.Price = getPackageDetailsById.Price;
                addEditfoodModel.Ingredient = getPackageDetailsById.Ingredient;
                addEditfoodModel.IsJainAvailable = getPackageDetailsById.IsJainAvailable;
                addEditfoodModel.IsBestSeller = getPackageDetailsById.IsBestSeller;
                addEditfoodModel.IsVegetarian = getPackageDetailsById.IsVegetarian;
                addEditfoodModel.FoodImageName = getPackageDetailsById.FoodImageName;
                addEditfoodModel.IsAvailable = getPackageDetailsById.IsAvailable;
                addEditfoodModel.DisplayOrder = getPackageDetailsById.DisplayOrder;
                addEditfoodModel.DiscountInPercentage = getPackageDetailsById.DiscountInPercentage;
            }
            else
            {
                ViewBag.IsReadOnlyClass = "";
            }
            ViewBag.ImageSRC = _imageFolderPath.Replace("\\","/");
            return View("_AddEditFood", addEditfoodModel);
        }

        [HttpPost]
        [Route("restaurant/add-edit-food")]
        public IActionResult AddEditFood(AddEditFoodViewModel addEditFoodViewModel, IFormFile ImageName)
        {
            try
            {
                if (addEditFoodViewModel.FoodId> 0)
                {
                    String FoodImageName1 = "";
                    var Extension = "";
                    var NewFileName = "";
                    if (ImageName != null) {
                         FoodImageName1 = GetTimestamp(DateTime.Now);
                        Extension = Path.GetExtension(ImageName.FileName);
                        NewFileName = FoodImageName1 + Extension;
                    }
                    
                    AddEditFoodResponse addEditFoodResponse = new AddEditFoodResponse();
                    addEditFoodResponse = objDatabaseRestaurant.AddEditFood(addEditFoodViewModel, GetCurrentRestaurant().RestaurantID, NewFileName);
                    if (addEditFoodResponse.status == 201)
                    {
                        if (ImageName != null)
                        {
                            // var fileName = FoodImageName1 + "_" + addEditFoodResponse.FoodItemID + Path.GetExtension(ImageName.FileName);
                            var fileName = FoodImageName1 + Path.GetExtension(ImageName.FileName);
                            var filePath = Path.Combine(_imageFolderPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                ImageName.CopyToAsync(stream);
                            }
                        }
                        return RedirectToAction("FoodItemList", new { Status = addEditFoodResponse.status });
                    }
                    else
                    {
                        return View("_AddEditFood", addEditFoodViewModel);
                    }
                }
                else
                {
                    if ((ImageName == null || ImageName.Length == 0) && addEditFoodViewModel.FoodId <= 0)
                    {
                        return BadRequest("No file uploaded.");
                    }
                    String FoodImageName1 = GetTimestamp(DateTime.Now);
                    var Extension = Path.GetExtension(ImageName.FileName);
                    var NewFileName = FoodImageName1 + Extension;
                    AddEditFoodResponse addEditFoodResponse = new AddEditFoodResponse();
                    addEditFoodResponse = objDatabaseRestaurant.AddEditFood(addEditFoodViewModel, GetCurrentRestaurant().RestaurantID, NewFileName);
                    if (addEditFoodResponse.status == 200)
                    {
                        // var fileName = FoodImageName1 + "_" + addEditFoodResponse.FoodItemID + Path.GetExtension(ImageName.FileName);
                        var fileName = FoodImageName1 + Path.GetExtension(ImageName.FileName);
                        var filePath = Path.Combine(_imageFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageName.CopyToAsync(stream);
                        }
                        return RedirectToAction("FoodItemList", new { Status = addEditFoodResponse.status });
                    }
                    else
                    {
                        return View("_AddEditFood", addEditFoodViewModel);
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteFoodItem(int FoodItemId)
        {
            try
            {
                DeleteFoodResponse DeleteFoodResponse = new DeleteFoodResponse();
                DeleteFoodResponse = objDatabaseRestaurant.DeletePackage(FoodItemId);
                return Json(new { result = DeleteFoodResponse });
            }
            catch
            {
                throw;
            }
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
