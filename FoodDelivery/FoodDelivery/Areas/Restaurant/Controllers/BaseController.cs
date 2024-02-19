using FoodDelivery.Areas.Restaurant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FoodDelivery.Models;

namespace FoodDelivery.Areas.Restaurant.Controllers
{
    
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetComplexData<FoodDelivery.Models.RestaurantSession>(Common.SessionKeys.RestaurantSession) == null)
            {
                // Custome Error Code for session timeout on ajax request 
                if (IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Login" },
                                    { "Action", "SessionOut" }
                        });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Login" },
                                    { "Action", "Index" }
                        });
                }
            }
            else
            {
                RestaurantSession restaurantSession = HttpContext.Session.GetComplexData<RestaurantSession>(Common.SessionKeys.RestaurantSession);              
            }
        }

        public RestaurantSession GetCurrentRestaurant()
        {
            return HttpContext.Session.GetComplexData<RestaurantSession>(Common.SessionKeys.RestaurantSession);
        }
        
        public bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }
            if (request.Headers == null)
            {
                return false;
            }
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}

