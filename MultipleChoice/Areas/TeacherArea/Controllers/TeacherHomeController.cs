using Microsoft.AspNetCore.Mvc;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [Area("TeacherArea")]
    //[Authorize]
    public class TeacherHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
