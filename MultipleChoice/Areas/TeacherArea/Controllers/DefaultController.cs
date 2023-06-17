using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using System.Security.Principal;

namespace MultipleChoice.Areas.TeacherArea.Controllers
{
    [Area("TeacherArea")]
    public class DefaultController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public DefaultController(MultipleChoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("teacher") != null)
            {
                //success
                return RedirectToAction("Index", "TeacherHome");
            }
            return View();
        }

        //Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var account = (from _teacher in _context.Teachers.Where(x => x.IdTeacher == username && x.Password == password && x.IsDelete != 1)
                           join _teaching in _context.Teachings.Where(y => y.StartingDate <= DateTime.Now && y.EndingDate >= DateTime.Now) on _teacher.Id equals _teaching.IdTeacher
                           select new
                           {
                               Id = _teacher.Id,
                               TeacherName = _teacher.TeacherName
                           }).ToList();

            if (account == null || account.Count == 0)
            {
                //not success
                return View();
            }
            else
            {
                //success
                HttpContext.Session.SetInt32("teacher", account[0].Id);
                return RedirectToAction("Index", "TeacherHome");
            }

        }
    }
}
