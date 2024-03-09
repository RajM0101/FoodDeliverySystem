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
        private readonly string _RestImageFolderPath;

        public FoodController()
        {
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Food");
            Directory.CreateDirectory(_imageFolderPath);
            _RestImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "Restaurant");
            Directory.CreateDirectory(_RestImageFolderPath);
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
        #region Restaurant
        [Route("restaurant/change-restaurant-detail")]
        [HttpGet]
        public ActionResult GetRestaurantDetailById()
        {
            EditRestaurantModel restaurantRegistrationModel = new EditRestaurantModel();

            GetRestaurantDetailById getRestaurantDetailById = objDatabaseRestaurant.GetRestaurantDetailsById(GetCurrentRestaurant().RestaurantID);
            if (getRestaurantDetailById != null)
            {
                restaurantRegistrationModel.RestaurantID = getRestaurantDetailById.RestaurantID;
                restaurantRegistrationModel.OwnerName = getRestaurantDetailById.OwnerName;
                restaurantRegistrationModel.RestaurantName = getRestaurantDetailById.RestaurantName;
                restaurantRegistrationModel.MobileNo = getRestaurantDetailById.MobileNo;
                restaurantRegistrationModel.Email = getRestaurantDetailById.Email;
                restaurantRegistrationModel.ShopPlotNumber = getRestaurantDetailById.ShopPlotNumber;
                restaurantRegistrationModel.Floor = getRestaurantDetailById.Floor;
                restaurantRegistrationModel.BuildingName = getRestaurantDetailById.BuildingName;
                restaurantRegistrationModel.ZipCode = getRestaurantDetailById.ZipCode;
                restaurantRegistrationModel.RestaurantImageName = getRestaurantDetailById.RestaurantImageName;

                ViewBag.IsReadOnlyClass = "readonly";
            }
            else
            {
                ViewBag.IsReadOnlyClass = "";
            }
            ViewBag.ImageSRC = _RestImageFolderPath.Replace("\\", "/");
            return View("_EditRestaurantDetail", restaurantRegistrationModel);
        }
        [Route("restaurant/change-restaurant-detail")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeRestaurantDetail(EditRestaurantModel restaurantRegistrationModel, IFormFile ImageName)
        {
            try
            {
                string Result = "";
                String FoodImageName1 = "";
                var Extension = "";
                var NewFileName = "";
                if (ImageName != null)
                {
                    FoodImageName1 = GetTimestamp(DateTime.Now);
                    Extension = Path.GetExtension(ImageName.FileName);
                    NewFileName = FoodImageName1 + Extension;
                }

                restaurantRegistrationModel.RestaurantID = GetCurrentRestaurant().RestaurantID;
                Result = objDatabaseRestaurant.ChangeRestaurantDetail(restaurantRegistrationModel, NewFileName);

                if (ImageName != null)
                {
                    // var fileName = FoodImageName1 + "_" + addEditFoodResponse.FoodItemID + Path.GetExtension(ImageName.FileName);
                    var fileName = FoodImageName1 + Path.GetExtension(ImageName.FileName);
                    var filePath = Path.Combine(_RestImageFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageName.CopyTo(stream);
                    }
                }
                return RedirectToAction("GetRestaurantDetailById");

            }
            catch (Exception ex) { throw; }
        }
        [HttpPost]
        public ActionResult DeleteRestaurantImage(string ImageName)
        {
            try
            {
                string Path = _RestImageFolderPath + "\\" + ImageName;
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }

                bool IsDeleted = true;
                if (IsDeleted)
                {
                    return Json(new { Deleted = 1 });
                }
                else
                {
                    return Json(new { Deleted = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Deleted = 0 });
            }
        }
        #endregion
        #region Food

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
            ViewBag.ImageSRC = _imageFolderPath.Replace("\\", "/");
            return View("_AddEditFood", addEditfoodModel);
        }

        [HttpPost]
        [Route("restaurant/add-edit-food")]
        public IActionResult AddEditFood(AddEditFoodViewModel addEditFoodViewModel, IFormFile ImageName)
        {
            try
            {
                if (addEditFoodViewModel.FoodId > 0)
                {
                    String FoodImageName1 = "";
                    var Extension = "";
                    var NewFileName = "";
                    if (ImageName != null)
                    {
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
                                ImageName.CopyTo(stream);
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
                            ImageName.CopyTo(stream);
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
                DeleteFoodResponse = objDatabaseRestaurant.DeleteFood(FoodItemId);
                return Json(new { result = DeleteFoodResponse });
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteFoodImage(string ImageName)
        {
            try
            {
                string Path = _imageFolderPath + "\\" + ImageName;
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }

                bool IsDeleted = true;
                if (IsDeleted)
                {
                    return Json(new { Deleted = 1 });
                }
                else
                {
                    return Json(new { Deleted = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Deleted = 0 });
            }
        }
        #endregion

        #region Order
        [Route("restaurant/restaurant-order")]
        [HttpGet]
        public ActionResult RestaurantOrderList()
        {
            return View("_OrderList");
        }
        [HttpGet]
        public ActionResult GetRestaurantOrderList(JQueryDataTableParamModel param, string Name)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<OrderListModel> list = objDatabaseRestaurant.GetRestaurantOrderList(param, Name, GetCurrentRestaurant().RestaurantID, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.OrderId),
                        Convert.ToString(c.OrderDetailID),
                         Convert.ToString(c.FoodId),
                        c.Name,
                        Convert.ToString(c.Qauntity),
                        Convert.ToString(c.OrderDate),
                        Convert.ToString(c.Price),
                        Convert.ToString(c.OrderStatus)
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
        public List<OrderstatusList> GetOrderstatusList()
        {
            List<OrderstatusList> list = objDatabaseRestaurant.GetOrderstatusList();
            return list;
        }
        [Route("restaurant/change-order-status")]
        [HttpGet]
        public ActionResult GetOrderDetailByOrderId(int OrderDetailID)
        {
            OrderViewModel orderViewModel = new OrderViewModel();

            orderViewModel = objDatabaseRestaurant.GetOrderDetailByOrderId(OrderDetailID, GetCurrentRestaurant().RestaurantID);

            ViewBag.IsReadOnlyClass = "readonly";
            ViewBag.OrderStatusList = GetOrderstatusList();
            return View("_ChangeOrderStatus", orderViewModel);
        }
        [HttpPost]
        [Route("restaurant/change-order-status")]
        public IActionResult ChangeOrderStatus(OrderViewModel orderViewModel)
        {
            try
            {
                if (ModelState.IsValid) {
                    OrderStatusResponse orderStatusResponse = new OrderStatusResponse();
                    orderStatusResponse = objDatabaseRestaurant.ChangeOrderStatus(orderViewModel, GetCurrentRestaurant().RestaurantID);
                    return RedirectToAction("RestaurantOrderList", new { Status = orderStatusResponse.status });
                }
                else
                {
                    return View("_ChangeOrderStatus", orderViewModel);
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
