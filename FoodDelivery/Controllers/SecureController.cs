using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Models;
using FoodDelivery.Areas.Restaurant.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace FoodDelivery.Controllers
{
    public class SecureController : BaseController
    {
        Database objDatabase = new Database();
        SessionUser sessionUser = new SessionUser();
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult AddFoodToCart(int FoodId, int Qauntity = 1)
        {
            try
            {
                RetriveDeatilFromCartModel retriveDeatilFromCart = objDatabase.AddFoodToCart(GetCurrentUser().UserId, FoodId, Qauntity);
                return Json(new { Data = retriveDeatilFromCart });
            }
            catch
            {
                throw;
            }
        }

        public ActionResult RemoveFoodFromCart(int FoodId, bool IsDeleteFromCart = false)
        {
            try
            {
                int result = objDatabase.RemoveFoodFromCart(GetCurrentUser().UserId, FoodId, IsDeleteFromCart);

                if (IsDeleteFromCart == false)
                {
                    return Json(new { RemoveFoodFromCart = result });
                }
                else
                {
                    return ViewComponent("CartDetail");
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("/cart-detail", Name = "CartDetail")]
        public ActionResult GetCartDetail()
        {
            CartListModel cartDetailList = objDatabase.GetCartsDetailsByUserId(GetCurrentUser().UserId);

            var TotalPrice = cartDetailList.cartTotalPrice.TotalPrice.ToString("N0");
            ViewBag.TotalPrices = TotalPrice;
            return View("_CartDetail", cartDetailList);
        }
        [HttpPost]
        public ActionResult AddUserOrder(string FoodIds)
        {
            try
            {
                AddOrderResoponse AddOrderResoponse=new AddOrderResoponse();
                AddOrderResoponse = objDatabase.AddOrder(FoodIds, GetCurrentUser().UserId);
                return Json(new { Result = AddOrderResoponse.RateStatus });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("/order-history", Name = "OrderHistory")]
        public ActionResult OrderHistory()
        {
            return View("_OrderHistory");
        }

        [HttpGet]
        public ActionResult GetUserOrderList(JQueryDataTableParamModel param)
        {
            try
            {
                IEnumerable<string[]> obj = Enumerable.Empty<string[]>();
                int noOfRecords;
                var SortOrderString = param.sColumns.Split(',');
                param.iSortCol_0 = SortOrderString[Convert.ToInt32(param.iSortCol_0)];
                List<GetUserOrderModel> list = objDatabase.GetUserOrderList(param, GetCurrentUser().UserId,out noOfRecords);
                obj = from c in list
                      select new[]
                      {
                        Convert.ToString(c.UserId),
                        Convert.ToString(c.OrderId),
                        Convert.ToString(c.OrderDetailID),
                        c.Name,
                        Convert.ToString(c.Price),
                        Convert.ToString(c.Qauntity),//5
                        c.TotalPrice,//6
                        c.OrderDate,//7
                        c.Rate.ToString()//8
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
        [HttpPost]
        public ActionResult RateToFood(int OrderDetailId,string Rate) {
            try
            {
                RateFoodResoponse rateFoodResoponse = new RateFoodResoponse();
                rateFoodResoponse = objDatabase.RateToFood(OrderDetailId, Rate,GetCurrentUser().UserId);
                return Json(new { Result = rateFoodResoponse.Status });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class CartDetailViewComponent : ViewComponent
    {
        Database objDatabase = new Database();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sessionUser = HttpContext.Session.GetComplexData<SessionUser>(Common.SessionKeys.UserSession);
            CartListModel cartDetailList = new CartListModel();
            if (sessionUser != null)
            {
                cartDetailList = objDatabase.GetCartsDetailsByUserId(sessionUser.UserId);
                var TotalPrice = cartDetailList.cartTotalPrice.TotalPrice.ToString("N0");
                ViewBag.TotalPrices = TotalPrice;
            }
            return View("CartDetail", cartDetailList);
        }
    }
}
