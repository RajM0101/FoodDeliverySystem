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

            restaurantViewModel = objDatabaseAdminUser.GetOrderDetailByOrderId(RestaurantID);

            ViewBag.IsReadOnlyClass = "readonly";
            return View("_ChangeRestaurantStatus", restaurantViewModel);
        }
        [HttpPost]
        [Route("adminuser/change-restaurant-status")]
        public IActionResult ChangeRestaurantStatus(RestaurantViewModel restaurantViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RestaurantStatusResponse StatusResponse = new RestaurantStatusResponse();
                    StatusResponse = objDatabaseAdminUser.ChangeOrderStatus(restaurantViewModel, GetCurrentAdminUser().AdminUserID);
                    return RedirectToAction("RestaurantList", new { Status = StatusResponse.status });
                }
                else
                {
                    return View("_ChangeOrderStatus", restaurantViewModel);
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
    }
}
