using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using System.Security.Principal;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            if (HttpContext.Session.GetString("admin") != null)
            {
                //success
                return RedirectToAction("Index", "AdminHome", new { area = "admin" });
            }
            return View();
        }

        //Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var account = _context.Admins.SingleOrDefault(x => x.Username == username);

            if (account != null && BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                //success
                HttpContext.Session.SetString("admin", account.Username);
                return RedirectToAction("Index", "AdminHome", new { area = "admin" });
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
            if (HttpContext.Session.GetString("admin") != null)
            {
                //success
                HttpContext.Session.Remove("admin");
                return RedirectToAction("Login", "Default", new { area = "admin" });
            }
            return null;
        }

        //Change password 
        [HttpPost]
        public JsonResult ChangePassword(string password)
        {
            try
            {
                var _username = HttpContext.Session.GetString("admin");

                var _account = _context.Admins.SingleOrDefault(x => x.Username == _username);

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
