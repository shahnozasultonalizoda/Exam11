using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcApp.Filters;

public class JwtSessionAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Session.GetString("Token");
        var role = context.HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (role != "Admin")
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        base.OnActionExecuting(context);
    }
}
