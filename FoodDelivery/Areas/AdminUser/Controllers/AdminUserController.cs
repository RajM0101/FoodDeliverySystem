using FoodDelivery.Areas.AdminUser.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Areas.AdminUser.Controllers;

namespace FoodDelivery.Areas.AdminUser.Controllers
{
    [Area("adminuser")]
    public class AdminUserController : AdminBaseController
    {
        DatabaseAdminUser objDatabaseAdminUser = new DatabaseAdminUser();
        #region Order
        [Route("adminuser/restaurants")]
        [HttpGet]
        public ActionResult RestaurantList()
        {
            return View("_RestaurantList");
        }
        [HttpGet]
        public ActionResult GetRestaurantList(JQueryDataTableParamModel param, string Name)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<RestaurantListModel> list = objDatabaseAdminUser.GetRestaurantList(param, Name,out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.RestaurantID),
                        Convert.ToString(c.OwnerName),
                         Convert.ToString(c.RestaurantName),
                        c.MobileNo,
                        Convert.ToString(c.Email),
                        Convert.ToString(c.Address),
                        c.ZipCode,
                        c.RestaurantStatus==false ?"Not Approved":"Approved"
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
        [Route("adminuser/change-restaurant-status")]
        [HttpGet]
        public ActionResult GetRestaurantDetailByOrderId(int RestaurantID)
        {
            RestaurantViewModel restaurantViewModel = new RestaurantViewModel();

            restaurantViewModel = objDatabaseAdminUser.GetRestaurantDetailByRestaurantId(RestaurantID);

            ViewBag.IsReadOnlyClass = "readonly";
            ViewBag.Name = "Restaurant";
            return View("_ChangeRestaurantStatus", restaurantViewModel);
        }
        [HttpPost]
        [Route("adminuser/change-restaurant-status")]
        public IActionResult ChangeRestaurantStatus(RestaurantViewModel restaurantViewModel)
        {
            try
            {
                ModelState.Remove("RestaurantImageName");
                if (ModelState.IsValid)
                {
                    RestaurantStatusResponse StatusResponse = new RestaurantStatusResponse();
                    StatusResponse = objDatabaseAdminUser.ChangeOrderStatus(restaurantViewModel, GetCurrentAdminUser().AdminUserID);
                    return RedirectToAction("RestaurantList", new { Status = StatusResponse.status });
                }
                else
                {
                    return View("_ChangeRestaurantStatus", restaurantViewModel);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Userlist
        [Route("adminuser/userlist")]
        [HttpGet]
        public ActionResult UserList()
        {
            return View("_UserList");
        }
        [HttpGet]
        public ActionResult GetUserList(JQueryDataTableParamModel param, string Name)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<UserListModel> list = objDatabaseAdminUser.GetUserList(param, Name, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.UserID),
                        Convert.ToString(c.Name),
                        c.MobileNo,
                        Convert.ToString(c.Address)
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
        #endregion
        #region Foodlist
        [Route("adminuser/foodlist")]
        [HttpGet]
        public ActionResult FoodList(string RestaurantID,string Name)
        {
            ViewBag.RestaurantID = RestaurantID;
            ViewBag.Name = Name;
            return View("_FoodList");
        }
        [HttpGet]
        public ActionResult GetFoodList(JQueryDataTableParamModel param,string RestaurantID, string Name)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<FoodListModel> list = objDatabaseAdminUser.GetFoodList(param, RestaurantID, Name, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.RestaurantID),
                        Convert.ToString(c.FoodID),
                        c.FoodName,
                        c.Price.ToString(),
                        c.Ingredient,
                        c.IsBestSeller==true ?"Yes":"No",
                        c.IsVegetarian==true?"Vegetarian":"Non-Veg",
                        Convert.ToString(c.Rate)
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
        #endregion
        [Route("adminuser/tiffinservices")]
        [HttpGet]
        public ActionResult TiffinServicesList()
        {
            return View("_TiffinServicesList");
        }
        [HttpGet]
        public ActionResult GetTiffinServicesList(JQueryDataTableParamModel param, string Name)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<TiffinServicesListModel> list = objDatabaseAdminUser.GetTiffinServicesList(param, Name, out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.TiffinServicesID),
                        Convert.ToString(c.OwnerName),
                         Convert.ToString(c.TiffinServicesName),
                        c.MobileNo,
                        Convert.ToString(c.Email),
                        Convert.ToString(c.Address),
                        c.ZipCode,
                        c.TiffinServicesStatus==false ?"Not Approved":"Approved"
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
        [Route("adminuser/change-tiffinservices-status")]
        [HttpGet]
        public ActionResult GetTiffinServicesDetailByOrderId(int TiffinServicesID)
        {
            RestaurantViewModel restaurantViewModel = new RestaurantViewModel();

            restaurantViewModel = objDatabaseAdminUser.GetTiffinServicesDetailByOrderId(TiffinServicesID);

            ViewBag.IsReadOnlyClass = "readonly";
            ViewBag.Name = "Tiffin Services";
            return View("_ChangeRestaurantStatus", restaurantViewModel);
        }

    }
}
