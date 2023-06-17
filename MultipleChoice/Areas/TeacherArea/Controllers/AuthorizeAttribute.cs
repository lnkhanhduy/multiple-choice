using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetInt32("teacher") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Default", new { area = "TeacherArea" });
            }
        }
    }
}
