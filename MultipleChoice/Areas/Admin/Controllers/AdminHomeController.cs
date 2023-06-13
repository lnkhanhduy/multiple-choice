using Microsoft.AspNetCore.Mvc;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToAction("Login", "Default");
            }
            return View();

        }
    }
}
