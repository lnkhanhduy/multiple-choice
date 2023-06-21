using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using System.Security.Principal;
using BCrypt.Net;

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
                return RedirectToAction("Index", "TeacherHome", new { area = "TeacherArea" });
            }
            return View();
        }

        //Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var account = (from _teacher in _context.Teachers.Where(x => x.IdTeacher == username && x.IsDelete != 1)
                           join _teaching in _context.Teachings.Where(y => y.StartingDate <= DateTime.Now && y.EndingDate >= DateTime.Now) on _teacher.Id equals _teaching.IdTeacher
                           select new
                           {
                               Id = _teacher.Id,
                               TeacherName = _teacher.TeacherName,
                               Password = _teacher.Password,
                           }).ToList();

            //success
            if (account != null && account.Count != 0 && BCrypt.Net.BCrypt.Verify(password, account[0].Password))
            {
                HttpContext.Session.SetInt32("teacher", account[0].Id);
                return RedirectToAction("Index", "TeacherHome", new { area = "TeacherArea" });
            }
            else
            {
                //not success
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác";
                return View();
            }
        }

        //Logout
        [HttpGet]
        public IActionResult? Logout()
        {
            if (HttpContext.Session.GetInt32("teacher") != null)
            {
                //success
                HttpContext.Session.Remove("teacher");
                return RedirectToAction("Login", "Default", new { area = "teacherarea" });
            }
            return null;
        }

        //Change password 
        [HttpPost]
        public JsonResult ChangePassword(string password)
        {
            try
            {
                var _username = HttpContext.Session.GetInt32("teacher");

                var _account = _context.Teachers.SingleOrDefault(x => x.Id == _username);

                if (_account != null)
                {
                    _account.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    _context.SaveChanges();
                }
                else
                {
                    return Json(new
                    {
                        code = 500,
                        message = "Không tìm thấy tài khoản!"
                    });
                }

                return Json(new
                {
                    code = 200,
                    message = "Đổi mật khẩu thành công!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    message = "Đổi mật khẩu thất bại: " + ex.Message
                });
            }
        }

    }
}
