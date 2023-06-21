using Microsoft.AspNetCore.Mvc;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
