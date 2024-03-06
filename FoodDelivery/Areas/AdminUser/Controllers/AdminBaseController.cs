using FoodDelivery.Areas.Restaurant.Models;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodDelivery.Areas.AdminUser.Controllers
{
    public class AdminBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetComplexData<FoodDelivery.Models.AdminUserSession>(Common.SessionKeys.AdminSession) == null)
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
                AdminUserSession adminUserSession = HttpContext.Session.GetComplexData<AdminUserSession>(Common.SessionKeys.AdminSession);
            }
        }

        public AdminUserSession GetCurrentAdminUser()
        {
            return HttpContext.Session.GetComplexData<AdminUserSession>(Common.SessionKeys.AdminSession);
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
