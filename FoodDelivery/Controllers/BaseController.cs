using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FoodDelivery.Models;
using FoodDelivery.Areas.Restaurant.Models;

namespace FoodDelivery.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetComplexData<SessionUser>(Common.SessionKeys.UserSession) == null)
            {
                // Custome Error Code for session timeout on ajax request 
                if (IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Home" },
                                    { "Action", "SessionOut" },
                                    { "Area", null }
                        });
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                    { "Controller", "Home" },
                                    { "Action", "Index" },
                                    { "Area", null }
                        });
                }
            }
        }
        public SessionUser GetCurrentUser()
        {
            return HttpContext.Session.GetComplexData<SessionUser>(Common.SessionKeys.UserSession);
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
