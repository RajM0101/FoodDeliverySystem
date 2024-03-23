using FoodDelivery.Areas.TiffinServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FoodDelivery.Models;

namespace FoodDelivery.Areas.TiffinServices.Controllers
{
    public class TiffinServicesBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetComplexData<FoodDelivery.Models.TiffinServicesSession>(Common.SessionKeys.TiffinServicesSession) == null)
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
                TiffinServicesSession tiffinServicesSession = HttpContext.Session.GetComplexData<TiffinServicesSession>(Common.SessionKeys.TiffinServicesSession);
            }
        }

        public TiffinServicesSession GetCurrentTiffinServices()
        {
            return HttpContext.Session.GetComplexData<TiffinServicesSession>(Common.SessionKeys.TiffinServicesSession);
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
